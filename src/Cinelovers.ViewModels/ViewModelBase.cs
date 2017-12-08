using ReactiveUI;
using Splat;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public string UrlPathSegment { get; protected set; }

        public IScreen HostScreen { get; protected set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public IScheduler TaskPoolScheduler { get; }

        public IScheduler MainScheduler { get; }

        public ViewModelBase(
            IScreen hostScreen = null,
            IScheduler mainScheduler = null, 
            IScheduler taskPoolScheduler = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
            MainScheduler = mainScheduler ?? Locator.Current.GetService<IScheduler>("MainScheduler");
            TaskPoolScheduler = taskPoolScheduler ?? Locator.Current.GetService<IScheduler>("TaskPoolScheduler");
        }
    }
}
