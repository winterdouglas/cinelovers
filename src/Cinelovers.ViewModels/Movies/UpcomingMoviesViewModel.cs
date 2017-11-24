using Cinelovers.Core.Services;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        public ReactiveList<MovieCellViewModel> Movies { get; } = new ReactiveList<MovieCellViewModel>();

        private readonly IMovieService _movieService;

        public UpcomingMoviesViewModel(
            IMovieService movieService = null,
            IScheduler mainScheduler = null,
            IScheduler taskPoolScheduler = null,
            IScreen hostScreen = null) 
            : base(mainScheduler, taskPoolScheduler, hostScreen)
        {
            _movieService = movieService ?? Locator.Current.GetService<IMovieService>();

            _movieService
                .GetUpcomingMovies(1)
                .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                .SubscribeOn(TaskPoolScheduler)
                .SelectMany(movies => movies)
                .ObserveOn(MainScheduler)
                .Subscribe(item => Movies.Add(item));
        }
    }
}
