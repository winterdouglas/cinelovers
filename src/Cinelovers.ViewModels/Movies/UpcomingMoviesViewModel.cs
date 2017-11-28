using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        public ReactiveCommand<int, IEnumerable<Movie>> GetUpcomingMovies { get; protected set; }

        public ReactiveCommand<int, IEnumerable<Movie>> GetMovies { get; protected set; }

        public ReactiveList<MovieCellViewModel> Movies { get; } = new ReactiveList<MovieCellViewModel>();

        public MovieCellViewModel SelectedMovie
        {
            get { return _selectedMovie; }
            set { this.RaiseAndSetIfChanged(ref _selectedMovie, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        public bool IsLoading => _isLoading.Value;

        private string _searchTerm;
        private ObservableAsPropertyHelper<bool> _isLoading;
        private MovieCellViewModel _selectedMovie;
        private readonly IMovieService _movieService;

        public UpcomingMoviesViewModel(
            IMovieService movieService = null,
            IScheduler mainScheduler = null,
            IScheduler taskPoolScheduler = null,
            IScreen hostScreen = null)
            : base(hostScreen, mainScheduler, taskPoolScheduler)
        {
            _movieService = movieService ?? Locator.Current.GetService<IMovieService>();

            UrlPathSegment = "Upcoming Movies";

            var canGetMovies = this
                .WhenAnyValue(
                    x => x.SearchTerm,
                    term => !string.IsNullOrWhiteSpace(term) && term.Length > 2);

            GetUpcomingMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => ClearAndGetUpcomingMovies(page),
                    outputScheduler: MainScheduler);

            GetMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => ClearAndGetMovies(SearchTerm, page),
                    canExecute: canGetMovies,
                    outputScheduler: MainScheduler);

            GetUpcomingMovies
                .IsExecuting
                .Merge(GetMovies.IsExecuting, TaskPoolScheduler)
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: MainScheduler);

            GetUpcomingMovies
                .Merge(GetMovies, TaskPoolScheduler)
                .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                .SelectMany(movies => movies)
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(MainScheduler)
                .Subscribe(
                    movie =>
                    {
                        if (!Movies.Any(m => m.Id == movie.Id))
                            Movies.Add(movie);
                    });

            this.WhenAnyValue(x => x.SelectedMovie)
                .Where(selected => selected != null)
                .Select(selected => new MovieDetailsViewModel(selected))
                .ObserveOn(MainScheduler)
                .InvokeCommand<IRoutableViewModel, IRoutableViewModel>(
                    HostScreen.Router.Navigate);

            var searchChanged = this
                .WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromSeconds(1), TaskPoolScheduler)
                .DistinctUntilChanged()
                .Publish();

            searchChanged
                .Where(searchTerm => !string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .InvokeCommand(GetMovies);

            searchChanged
                .Where(searchTerm => string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .InvokeCommand(GetUpcomingMovies);

            searchChanged
                .Connect();

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                SelectedMovie = null;
            });
        }

        private IObservable<IEnumerable<Movie>> ClearAndGetUpcomingMovies(int page)
        {
            return Observable
                .Create<Unit>(
                    observer =>
                    {
                        if (page == 1)
                            MainScheduler.Schedule(() => Movies.Clear());

                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();

                        return Disposable.Empty;
                    })
                .ObserveOn(TaskPoolScheduler)
                .SelectMany(_ => _movieService.GetUpcomingMovies(page));
        }

        private IObservable<IEnumerable<Movie>> ClearAndGetMovies(string query, int page)
        {
            return Observable
                .Create<Unit>(
                    observer =>
                    {
                        if (page == 1)
                            MainScheduler.Schedule(() => Movies.Clear());

                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();

                        return Disposable.Empty;
                    })
                .ObserveOn(TaskPoolScheduler)
                .SelectMany(_ => _movieService.GetMovies(query, page));
        }
    }
}
