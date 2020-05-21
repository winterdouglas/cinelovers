using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Services;
using DynamicData;
using DynamicData.Binding;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        private readonly IMovieService _movieService;
        private MovieCellViewModel _selectedMovie;
        private string _searchTerm;

        public UpcomingMoviesViewModel(
            IMovieService movieService,
            INavigationService navigationService,
            ISchedulerService schedulerService)
            : base(navigationService, schedulerService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));

            LoadMovies = ReactiveCommand
                .CreateFromObservable<int, Unit>(
                    page =>
                    {
                        return SearchTerm?.Length > 2
                            ? _movieService.LoadMovies(SearchTerm, page)
                            : _movieService.LoadUpcomingMovies(page);
                    },
                    outputScheduler: SchedulerService.MainThread);

            _movieService
                .Movies
                .Connect()
                .Transform(movie => new MovieCellViewModel(movie))
                .ObserveOn(SchedulerService.MainThread)
                .Bind(Movies)
                .DisposeMany()
                .Subscribe()
                .DisposeWith(Disposables);

            Observable
                .Merge(
                    GetUpcomingMovies.IsExecuting,
                    LoadMovies.IsExecuting)
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: SchedulerService.MainThread)
                .DisposeWith(Disposables);

            this.WhenAnyValue(x => x.SelectedMovie)
                .Where(selected => selected != null)
                .ObserveOn(SchedulerService.MainThread)
                .SelectMany(selected => Observable
                    .FromAsync(() => NavigationService
                        .NavigateAsync($"details?id={selected.Id}", useModalNavigation: true)))
                .Subscribe()
                .DisposeWith(Disposables);

            this
                .WhenAnyValue(x => x.SearchTerm)
                .Throttle(TimeSpan.FromSeconds(2), SchedulerService.TaskPool)
                .DistinctUntilChanged()
                .InvokeCommand(LoadMovies)
                .DisposeWith(Disposables);

            Observable
                .Merge(
                    GetUpcomingMovies.ThrownExceptions,
                    LoadMovies.ThrownExceptions)
                .Subscribe(ex =>
                {
                    Console.WriteLine(ex);
                })
                .DisposeWith(Disposables);
        }

        public ReactiveCommand<int, Unit> GetUpcomingMovies { get; protected set; }

        public ReactiveCommand<int, Unit> LoadMovies { get; protected set; }

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
    }
}
