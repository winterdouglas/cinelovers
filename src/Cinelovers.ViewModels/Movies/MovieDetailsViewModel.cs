using ReactiveUI;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        public MovieDetailsViewModel(
            IScheduler mainScheduler = null, 
            IScheduler taskPoolScheduler = null, 
            IScreen hostScreen = null) 
            : base(mainScheduler, taskPoolScheduler, hostScreen)
        {
        }
    }
}
