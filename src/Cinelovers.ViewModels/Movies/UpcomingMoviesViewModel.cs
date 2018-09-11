using Cinelovers.Core.Services;
using DynamicData;
using DynamicData.PLinq;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DynamicData.ReactiveUI;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        public ReactiveCommand<int, Unit> GetUpcomingMovies { get; protected set; }

        public ReactiveCommand<int, Unit> GetMovies { get; protected set; }

        public ReactiveList<MovieCellViewModel> Movies { get; } = new ReactiveList<MovieCellViewModel>();

        public ReactiveList<MovieCellViewModel> UpcomingMovies { get; } = new ReactiveList<MovieCellViewModel>();

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

            var canSearch = this
                .WhenAnyValue(
                    x => x.SearchTerm,
                    term => !string.IsNullOrWhiteSpace(term) && term.Length > 2);

            GetUpcomingMovies = ReactiveCommand
                .CreateFromObservable<int, Unit>(
                    page => _movieService.GetUpcomingMovies(page),
                    outputScheduler: mainScheduler);

            GetMovies = ReactiveCommand
                .CreateFromObservable<int, Unit>(
                    page => _movieService.GetMovies(SearchTerm, page),
                    canExecute: canSearch,
                    outputScheduler: mainScheduler);

            GetUpcomingMovies.Subscribe();
            GetMovies.Subscribe();

            _movieService
                .Movies
                .Connect()
                .Transform(movie => new MovieCellViewModel(movie), new ParallelisationOptions(ParallelType.Ordered, 5))
                .Bind(Movies)
                .DisposeMany()
                .Subscribe();

            _movieService
                .UpcomingMovies
                .Connect()
                .Transform(movie => new MovieCellViewModel(movie), new ParallelisationOptions(ParallelType.Ordered, 5))
                .Bind(UpcomingMovies)
                .DisposeMany()
                .Subscribe();

            GetUpcomingMovies
                .IsExecuting
                .Merge(GetMovies.IsExecuting, TaskPoolScheduler)
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: MainScheduler);

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
                .Publish();

            searchChanged
                .Where(searchTerm => !string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(TaskPoolScheduler)
                .InvokeCommand(GetMovies);

            searchChanged
                .Skip(1)
                .Where(searchTerm => string.IsNullOrWhiteSpace(searchTerm))
                .Select(_ => 1)
                .SubscribeOn(TaskPoolScheduler)
                .InvokeCommand(GetUpcomingMovies);

            searchChanged
                .Connect();
        }
    }
}
