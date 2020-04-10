using Cinelovers.ViewModels;
using Prism.Navigation;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;

namespace Cinelovers.Views
{
    public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>, IDestructible
        where TViewModel : ViewModelBase
    {
        public ContentPageBase()
        {
            Disposables = new CompositeDisposable();
        }

        protected CompositeDisposable Disposables { get; }

        public void Destroy()
        {
            Disposables.Dispose();
        }
    }
}
