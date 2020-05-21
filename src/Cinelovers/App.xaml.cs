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
using Newtonsoft.Json;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Refit;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Cinelovers
{
    public partial class App : PrismApplication
    {
        private const string ApplicationName = "Cinelovers";
        private const string ApiBaseUri = "https://api.themoviedb.org/3";

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
            containerRegistry.RegisterSingleton<IApiCache, AkavacheApiCache>();
            containerRegistry.RegisterInstance(CreateApiClient());
            containerRegistry.RegisterSingleton<IMovieService, MovieService>();
        }

        protected override void OnStart()
        {
            AppCenter.Start(
                $"android={Secrets.AppCenterAndroid};" +
                $"ios={Secrets.AppCenterIos}",
                typeof(Analytics), typeof(Crashes));

            base.OnStart();
        }

        protected override void OnResume()
        {
            Container
                .Resolve<IApiCache>()
                .Initialize(ApplicationName);

            base.OnResume();
        }

        protected override void OnSleep()
        {
            Container
                .Resolve<IApiCache>()
                .Shutdown();

            base.OnSleep();
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

        private ITmdbApiClient CreateApiClient()
        {
            return RestService
                .For<ITmdbApiClient>(
                    Container
                        .Resolve<IHttpClientFactory>()
                        .CreateClient(Priority.UserInitiated, ApiBaseUri),
                    new RefitSettings
                    {
                        ContentSerializer = new NewtonsoftJsonContentSerializer(
                            new JsonSerializerSettings
                            {
                                ContractResolver = new SnakeCaseContractResolver()
                            })
                    });
        }
    }
}
