using ReactiveUI;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        public MovieDetailsViewModel(
            IScreen hostScreen = null,
            IScheduler mainScheduler = null, 
            IScheduler taskPoolScheduler = null) 
            : base(hostScreen, mainScheduler, taskPoolScheduler)
        {
        }
    }
}
