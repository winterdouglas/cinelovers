using Android.App;
using Android.Content.PM;
using Android.OS;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Droid.Infrastructure;
using FFImageLoading.Forms.Platform;
using Splat;
using Xamarin.Forms;

namespace Cinelovers.Droid
{
    [Activity(
        Label = "Cinelovers",
        Icon = "@drawable/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.User)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Locator.CurrentMutable.RegisterLazySingleton<IHttpClientFactory>(() => new HttpClientFactory());

            CachedImageRenderer.Init(true);

            Forms.SetFlags("CollectionView_Experimental");

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

