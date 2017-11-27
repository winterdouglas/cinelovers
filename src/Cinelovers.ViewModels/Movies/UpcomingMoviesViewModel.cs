using Cinelovers.Core.Services;
using Cinelovers.Core.Services.Models;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        public ReactiveCommand<int, IEnumerable<Movie>> GetUpcomingMovies { get; protected set; }

        public ReactiveList<MovieCellViewModel> Movies { get; } = new ReactiveList<MovieCellViewModel>();

        public MovieCellViewModel SelectedMovie
        {
            get { return _selectedMovie; }
            set { this.RaiseAndSetIfChanged(ref _selectedMovie, value); }
        }

        public bool IsLoading => _isLoading.Value;

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

            GetUpcomingMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => _movieService.GetUpcomingMovies(page));

            GetUpcomingMovies
                .IsExecuting
                .ToProperty(this, x => x.IsLoading, out _isLoading,
                    scheduler: MainScheduler);

            GetUpcomingMovies
                .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                .SelectMany(movies => movies)
                .Where(movie => !Movies.Select(m => m.Id).ToList().Contains(movie.Id))
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(MainScheduler)
                .Subscribe(item => Movies.Add(item));

            this.WhenAnyValue(x => x.SelectedMovie)
                .Where(selected => selected != null)
                .Select(selected => new MovieDetailsViewModel(selected))
                .ObserveOn(MainScheduler)
                .InvokeCommand<IRoutableViewModel, IRoutableViewModel>(
                    HostScreen.Router.Navigate);

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                SelectedMovie = null;
            });
        }
    }
}
