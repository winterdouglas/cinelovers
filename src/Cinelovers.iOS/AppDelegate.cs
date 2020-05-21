using Foundation;
using UIKit;
using FFImageLoading.Forms.Platform;
using Cinelovers.Core.Infrastructure;
using Cinelovers.iOS.Infrastructure;
using Prism;
using Prism.Ioc;

namespace Cinelovers.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IPlatformInitializer
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            LoadApplication(new App(this));

            return base.FinishedLaunching(app, options);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHttpClientFactory, HttpClientFactory>();
        }
    }
}
