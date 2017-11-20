using Cinelovers.ViewModels.Movies;
using Cinelovers.Views.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System;
using Xamarin.Forms;

namespace Cinelovers
{
    public class Bootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; protected set; }

        public Bootstrapper()
        {
            Router = new RoutingState();

            RegisterDependencies();

            Router
                .NavigateAndReset
                .Execute(new UpcomingMoviesViewModel())
                .Subscribe();
        }

        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }

        private void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            Locator.CurrentMutable.Register(() => new UpcomingMoviesView(), typeof(IViewFor<UpcomingMoviesViewModel>));
            Locator.CurrentMutable.Register(() => new MovieDetailsView(), typeof(IViewFor<MovieDetailsViewModel>));
        }
    }
}
