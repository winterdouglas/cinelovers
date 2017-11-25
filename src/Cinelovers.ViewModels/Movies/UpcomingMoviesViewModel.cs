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

        public UpcomingMoviesViewModel(
            IMovieService movieService = null,
            IScheduler mainScheduler = null,
            IScheduler taskPoolScheduler = null,
            IScreen hostScreen = null) 
            : base(mainScheduler, taskPoolScheduler, hostScreen)
        {
            _movieService = movieService ?? Locator.Current.GetService<IMovieService>();

            this.WhenActivated(disposables =>
            {
                GetUpcomingMovies = ReactiveCommand
                    .CreateFromObservable<int, IEnumerable<Movie>>(
                        (page) => Observable.Throw<IEnumerable<Movie>>(new NotImplementedException()));

                GetUpcomingMovies
                    .Subscribe()
                    .DisposeWith(disposables);
            });
            //_movieService
            //    .GetUpcomingMovies(1)
            //    .Select(movies => movies.Select(movie => new MovieCellViewModel(movie)))
            //    .SubscribeOn(TaskPoolScheduler)
            //    .SelectMany(movies => movies)
            //    .ObserveOn(MainScheduler)
            //    .Subscribe(item => Movies.Add(item));
        }
    }
}
