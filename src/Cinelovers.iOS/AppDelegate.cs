using Foundation;
using UIKit;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;
using Splat;
using Cinelovers.Core.Infrastructure;
using Cinelovers.iOS.Infrastructure;

namespace Cinelovers.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Locator.CurrentMutable.RegisterLazySingleton<IHttpClientFactory>(() => new HttpClientFactory());

            CachedImageRenderer.Init();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
