using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using DynamicData.Binding;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ReactiveCommand<Unit, int> Load { get; protected set; }

        public ObservableCollectionExtended<MovieCellViewModel> Movies { get; } = new ObservableCollectionExtended<MovieCellViewModel>();

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
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
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

            Load = ReactiveCommand
                .CreateFromObservable<Unit, int>(
                    _ => Observable.Create<int>(
                        observer =>
                        {
                            const int pageSize = 20;

                            if (Movies.Count % pageSize == 0)
                            {
                                observer.OnNext(Movies.Count + 1);
                            }
                            observer.OnCompleted();

                            return Disposable.Empty;
                        }),
                    outputScheduler: MainScheduler);

            var loadRequested = Load
                .Publish()
                .RefCount();

            loadRequested
                .Where(page => string.IsNullOrWhiteSpace(SearchTerm))
                .StartWith(1)
                .DistinctUntilChanged()
                .InvokeCommand(GetUpcomingMovies);

            loadRequested
                .Where(page => !string.IsNullOrWhiteSpace(SearchTerm))
                .DistinctUntilChanged()
                .InvokeCommand(GetMovies);

            GetUpcomingMovies
                .IsExecuting
                .Merge(GetMovies.IsExecuting, TaskPoolScheduler)
                .SubscribeOn(TaskPoolScheduler)
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: MainScheduler);

            var moviesChanged = GetUpcomingMovies
                .Merge(GetMovies, TaskPoolScheduler)
                .Where(movies => movies != null)
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(TaskPoolScheduler)
                .Publish();

            moviesChanged
                .Select(movies => movies.Where(movie => Movies.Any(m => m.Id == movie.Id)))
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(MainScheduler)
                .SelectMany(movies => MergeMovies(movies))
                .Subscribe();

            moviesChanged
                .Select(movies => movies.Where(movie => !Movies.Any(m => m.Id == movie.Id)))
                .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(MainScheduler)
                .Subscribe(movies => Movies.AddRange(movies));

            moviesChanged
                .Connect();

            this.WhenAnyValue(x => x.SelectedMovie)
                .Where(selected => selected != null)
                .Select(selected => new MovieDetailsViewModel(selected))
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(MainScheduler)
                .InvokeCommand<IRoutableViewModel, IRoutableViewModel>(
                    HostScreen.Router.Navigate);

            var searchChanged = this
                .WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromSeconds(1), TaskPoolScheduler)
                .DistinctUntilChanged()
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(TaskPoolScheduler)
                .Publish();

            searchChanged
                .Where(searchTerm => !string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(TaskPoolScheduler)
                .InvokeCommand(GetMovies);

            searchChanged
                .Skip(1)
                .Where(searchTerm => string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(TaskPoolScheduler)
                .InvokeCommand(GetUpcomingMovies);

            searchChanged
                .Connect();

            GetUpcomingMovies
                .ThrownExceptions
                .Merge(GetMovies.ThrownExceptions)
                .Subscribe(ex =>
                {
                    Console.WriteLine(ex);
                });

        }

        private IObservable<IEnumerable<Movie>> ClearAndGetUpcomingMovies(int page)
        {
            return Observable
                .Create<IEnumerable<Movie>>(
                    observer =>
                    {
                        if (page == 1)
                            MainScheduler.Schedule(() => Movies.Clear());

                        return _movieService
                            .GetUpcomingMovies(page)
                            .Subscribe(observer);
                    });
        }

        private IObservable<IEnumerable<Movie>> ClearAndGetMovies(string query, int page)
        {
            return Observable
                .Create<IEnumerable<Movie>>(
                    observer =>
                    {
                        if (page == 1)
                            MainScheduler.Schedule(() => Movies.Clear());

                        return _movieService
                            .GetMovies(query, page)
                            .Subscribe(observer);
                    });
        }

        private IObservable<Unit> MergeMovies(IEnumerable<Movie> movies)
        {
            return Observable
                .Create<Unit>(
                    observer =>
                    {
                        foreach (var movie in movies)
                        {
                            var foundMovie = Movies.First(m => m.Id == movie.Id);
                            foundMovie.UpdateMovie(movie);
                        }
                        observer.OnNext(Unit.Default);
                        observer.OnCompleted();

                        return Disposable.Empty;
                    });
        }
    }
}
