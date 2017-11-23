using Cinelovers.Core.Services;
using ReactiveUI;
using System;

namespace Cinelovers.ViewModels.Movies
{
    public class UpcomingMoviesViewModel : ViewModelBase
    {
        public UpcomingMoviesViewModel(IScreen hostScreen = null) : base(hostScreen)
        {
            new MovieService()
                .GetUpcomingMovies(1)
                .Subscribe();
        }
    }
}
