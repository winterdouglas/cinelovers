using ReactiveUI;
using Splat;
using System.Reactive.Concurrency;

namespace Cinelovers.ViewModels
{
    public class ViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public string UrlPathSegment { get; protected set; }

        public IScreen HostScreen { get; protected set; }

        public ViewModelActivator Activator => new ViewModelActivator();

        protected IScheduler TaskPoolScheduler { get; }

        protected IScheduler MainScheduler { get; }

        public ViewModelBase(
            IScheduler mainScheduler = null, 
            IScheduler taskPoolScheduler = null, 
            IScreen hostScreen = null)
        {
            MainScheduler = mainScheduler ?? Locator.Current.GetService<IScheduler>("MainScheduler");
            TaskPoolScheduler = taskPoolScheduler ?? Locator.Current.GetService<IScheduler>("TaskPoolScheduler");
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
