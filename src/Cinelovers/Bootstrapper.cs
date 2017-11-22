using Cinelovers.Core.Caching;
using Cinelovers.Core.Rest;
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
            var cache = new AkavacheCache();
            cache.Initialize(App.CacheKey);

            Locator.CurrentMutable.RegisterConstant<IScreen>(this);
            Locator.CurrentMutable.RegisterConstant<ICache>(cache);
            Locator.CurrentMutable.Register<ITmdbApiService>(() => new TmdbApiService());
            Locator.CurrentMutable.Register<IViewFor<UpcomingMoviesViewModel>>(() => new UpcomingMoviesView());
            Locator.CurrentMutable.Register<IViewFor<MovieDetailsViewModel>>(() => new MovieDetailsView());
        }
    }
}
