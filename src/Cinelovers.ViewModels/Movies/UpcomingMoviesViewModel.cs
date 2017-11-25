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

        private readonly IMovieService _movieService;

        public UpcomingMoviesViewModel() : this(null)
        {
        }

        public UpcomingMoviesViewModel(
            IMovieService movieService = null,
            IScheduler mainScheduler = null,
            IScheduler taskPoolScheduler = null,
            IScreen hostScreen = null) 
            : base(hostScreen, mainScheduler, taskPoolScheduler)
        {
            _movieService = movieService ?? Locator.Current.GetService<IMovieService>();

            GetUpcomingMovies = ReactiveCommand
                .CreateFromObservable<int, IEnumerable<Movie>>(
                    page => _movieService.GetUpcomingMovies(page));

            this.WhenActivated(disposables =>
            {
                GetUpcomingMovies
                    .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
                    .SelectMany(movies => movies)
                    .SubscribeOn(TaskPoolScheduler)
                    .ObserveOn(MainScheduler)
                    .Subscribe(item => Movies.Add(item))
                    .DisposeWith(disposables);
            });
        }
    }
}
