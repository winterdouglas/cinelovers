using Cinelovers.Core.Caching;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Services;
using Cinelovers.ViewModels.Movies;
using Cinelovers.Views.Movies;
using FFImageLoading;
using FFImageLoading.Config;
using Fusillade;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using System;
using System.Net.Http;
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
            var httpClientFactory = Locator.Current.GetService<IHttpClientFactory>();
            Locator.CurrentMutable.RegisterLazySingleton(() => httpClientFactory.CreateClient(Priority.Background), "ImageClient");
            Locator.CurrentMutable.RegisterLazySingleton(() => httpClientFactory.CreateClient(Priority.UserInitiated), "ApiClient");

            var config = new Configuration
            {
                DiskCacheDuration = TimeSpan.FromDays(1),
                HttpClient = Locator.Current.GetService<HttpClient>("ImageClient")
            };
            ImageService.Instance.Initialize(config);

            Locator.CurrentMutable.RegisterConstant(RxApp.MainThreadScheduler, "MainScheduler");
            Locator.CurrentMutable.RegisterConstant(RxApp.TaskpoolScheduler, "TaskPoolScheduler");
            Locator.CurrentMutable.RegisterConstant<IScreen>(this);
            Locator.CurrentMutable.RegisterLazySingleton<ICache>(() =>
            {
                var cache = new AkavacheCache();
                cache.Initialize(App.CacheKey);
                return cache;
            });
            Locator.CurrentMutable.RegisterLazySingleton<ITmdbApiService>(() => new TmdbApiService(Locator.Current.GetService<HttpClient>("ApiClient")));
            Locator.CurrentMutable.RegisterLazySingleton<IMovieService>(() => new MovieService());
            Locator.CurrentMutable.Register<IViewFor<UpcomingMoviesViewModel>>(() => new UpcomingMoviesView());
            Locator.CurrentMutable.Register<IViewFor<MovieDetailsViewModel>>(() => new MovieDetailsView());
        }
    }
}
