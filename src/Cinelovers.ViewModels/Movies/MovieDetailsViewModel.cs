using ReactiveUI;
using System;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        public MovieCellViewModel Movie
        {
            get { return _movie; }
            protected set { this.RaiseAndSetIfChanged(ref _movie, value); }
        }

        private MovieCellViewModel _movie;

        public MovieDetailsViewModel(
            MovieCellViewModel movie,
            IScreen hostScreen = null,
            IScheduler mainScheduler = null, 
            IScheduler taskPoolScheduler = null) 
            : base(hostScreen, mainScheduler, taskPoolScheduler)
        {
            Movie = movie ?? throw new ArgumentNullException(nameof(movie));

            UrlPathSegment = "Movie Details";
        }
    }
}
