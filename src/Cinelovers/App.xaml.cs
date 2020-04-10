using Cinelovers.Core.Caching;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Services;
using Cinelovers.ViewModels.Movies;
using Cinelovers.Views.Movies;
using FFImageLoading;
using FFImageLoading.Config;
using Fusillade;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Net.Http;
using Xamarin.Forms;

namespace Cinelovers
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            SetupImageService();

            await NavigationService.NavigateAsync("nav/upcoming");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("nav");
            containerRegistry.RegisterForNavigation<UpcomingMoviesView, UpcomingMoviesViewModel>("upcoming");
            containerRegistry.RegisterForNavigation<MovieDetailsView, MovieDetailsViewModel>("details");

            containerRegistry.RegisterSingleton<ISchedulerService, DefaultSchedulerService>();
            containerRegistry.RegisterInstance(Container.Resolve<IHttpClientFactory>().CreateClient(Priority.UserInitiated));
            containerRegistry.RegisterSingleton<IApiCache, AkavacheApiCache>();
            containerRegistry.RegisterSingleton<ITmdbApiService, TmdbApiService>();
            containerRegistry.RegisterSingleton<IMovieService, MovieService>();
        }

        protected override void OnStart()
        {
            AppCenter.Start(
                $"android={Secrets.AppCenterAndroid};" +
                $"ios={Secrets.AppCenterIos}",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            Container.Resolve<IApiCache>().Shutdown();
        }

        private void SetupImageService()
        {
            var config = new Configuration
            {
                DiskCacheDuration = TimeSpan.FromDays(1),
                HttpClient = Container
                    .Resolve<IHttpClientFactory>()
                    .CreateClient(Priority.Background)
            };
            ImageService.Instance.Initialize(config);
        }
    }
}
