using Cinelovers.Core.Api;
using Cinelovers.Core.Services;
using Cinelovers.Infrastructure.Caching;
using Cinelovers.ViewModels.Movies;
using Cinelovers.Views.Movies;
using FFImageLoading;
using FFImageLoading.Config;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
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

            var config = new Configuration
            {
                DiskCacheDuration = TimeSpan.FromDays(1)
            };
            ImageService.Instance.Initialize(config);

            Observable
                .Return(new UpcomingMoviesViewModel())
                .InvokeCommand<IRoutableViewModel, IRoutableViewModel>(
                    Router.NavigateAndReset);
        }

        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }

        private void RegisterDependencies()
        {
            var cache = new AkavacheCache();
            cache.Initialize(App.CacheKey);

            Locator.CurrentMutable.RegisterConstant(RxApp.MainThreadScheduler, "MainScheduler");
            Locator.CurrentMutable.RegisterConstant(RxApp.TaskpoolScheduler, "TaskPoolScheduler");
            Locator.CurrentMutable.RegisterConstant<IScreen>(this);
            Locator.CurrentMutable.RegisterConstant<ICache>(cache);
            Locator.CurrentMutable.Register<IApiService>(() => new TmdbApiService());
            Locator.CurrentMutable.Register<IMovieService>(() => new MovieService());

            Locator.CurrentMutable.Register<IViewFor<UpcomingMoviesViewModel>>(() => new UpcomingMoviesView());
            Locator.CurrentMutable.Register<IViewFor<MovieDetailsViewModel>>(() => new MovieDetailsView());
        }
    }
}
