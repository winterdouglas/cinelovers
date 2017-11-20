using ReactiveUI;
using Splat;

namespace Cinelovers.ViewModels
{
    public class ViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public string UrlPathSegment { get; protected set; }

        public IScreen HostScreen { get; protected set; }

        public ViewModelActivator Activator => new ViewModelActivator();

        public ViewModelBase(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
