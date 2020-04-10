using Android.App;
using Android.Content.PM;
using Android.OS;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Droid.Infrastructure;
using FFImageLoading.Forms.Platform;
using Prism;
using Prism.Ioc;

namespace Cinelovers.Droid
{
    [Activity(
        Label = "Cinelovers",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.User)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IPlatformInitializer
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CachedImageRenderer.Init(true);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(this));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHttpClientFactory, HttpClientFactory>();
        }
    }
}

