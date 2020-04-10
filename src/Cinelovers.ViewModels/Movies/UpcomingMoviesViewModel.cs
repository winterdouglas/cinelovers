using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using Prism.Navigation;
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

        public ObservableCollection<MovieCellViewModel> Movies { get; } = new ObservableCollection<MovieCellViewModel>();

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
            IMovieService movieService,
            INavigationService navigationService,
            ISchedulerService schedulerService)
            : base(navigationService, schedulerService)
        {
            _movieService = movieService;

            var canGetMovies = this
                .WhenAnyValue(
                    x => x.SearchTerm,
                    term => !string.IsNullOrWhiteSpace(term) && term.Length > 2);

            GetUpcomingMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => ClearAndGetUpcomingMovies(page),
                    outputScheduler: SchedulerService.MainThread);

            GetMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => ClearAndGetMovies(SearchTerm, page),
                    canExecute: canGetMovies,
                    outputScheduler: SchedulerService.MainThread);

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
                    outputScheduler: SchedulerService.MainThread);

            var loadRequested = Load
                .Publish()
                .RefCount();

            loadRequested
                .Where(page => string.IsNullOrWhiteSpace(SearchTerm))
                .StartWith(1)
                .DistinctUntilChanged()
                .ObserveOn(SchedulerService.TaskPool)
                .InvokeCommand(GetUpcomingMovies);

            loadRequested
                .Where(page => !string.IsNullOrWhiteSpace(SearchTerm))
                .DistinctUntilChanged()
                .ObserveOn(SchedulerService.TaskPool)
                .InvokeCommand(GetMovies);

            GetUpcomingMovies
                .IsExecuting
                .Merge(GetMovies.IsExecuting, SchedulerService.TaskPool)
                .SubscribeOn(SchedulerService.TaskPool)
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: SchedulerService.MainThread);

            var moviesChanged = GetUpcomingMovies
                .Merge(GetMovies, SchedulerService.TaskPool)
                .Where(movies => movies != null)
                .SubscribeOn(SchedulerService.TaskPool)
                .ObserveOn(SchedulerService.TaskPool)
                .Publish();

            moviesChanged
                .Select(movies => movies.Where(movie => Movies.Any(m => m.Id == movie.Id)))
                .SubscribeOn(SchedulerService.TaskPool)
                .ObserveOn(SchedulerService.MainThread)
                .SelectMany(movies => MergeMovies(movies))
                .Subscribe();

            moviesChanged
                .Select(movies => movies.Where(movie => !Movies.Any(m => m.Id == movie.Id)))
                .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                .SubscribeOn(SchedulerService.TaskPool)
                .SelectMany(movie => movie)
                .ObserveOn(SchedulerService.MainThread)
                .Subscribe(movies => Movies.Add(movies));

            moviesChanged
                .Connect();

            //this.WhenAnyValue(x => x.SelectedMovie)
            //    .Where(selected => selected != null)
            //    .Select(selected => new MovieDetailsViewModel(selected))
            //    .SubscribeOn(TaskPoolScheduler)
            //    .ObserveOn(MainScheduler)
            //    .InvokeCommand<IRoutableViewModel, IRoutableViewModel>(
            //        HostScreen.Router.Navigate);

            var searchChanged = this
                .WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromSeconds(1), SchedulerService.TaskPool)
                .DistinctUntilChanged()
                .SubscribeOn(SchedulerService.TaskPool)
                .ObserveOn(SchedulerService.TaskPool)
                .Publish();

            searchChanged
                .Where(searchTerm => !string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(SchedulerService.TaskPool)
                .ObserveOn(SchedulerService.TaskPool)
                .InvokeCommand(GetMovies);

            searchChanged
                .Skip(1)
                .Where(searchTerm => string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(SchedulerService.TaskPool)
                .ObserveOn(SchedulerService.TaskPool)
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
                            SchedulerService.MainThread.Schedule(() => Movies.Clear());

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
                            SchedulerService.MainThread.Schedule(() => Movies.Clear());

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
