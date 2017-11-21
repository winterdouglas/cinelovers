using Cinelovers.Core.Caching;
using Splat;
using Xamarin.Forms;

namespace Cinelovers
{
    public partial class App : Application
    {
        public const string CacheKey = "Cinelovers";

        public App()
        {
            InitializeComponent();

            var bootstrapper = new Bootstrapper();
            MainPage = bootstrapper.CreateMainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            var cache = Locator.Current.GetService<ICache>();
            cache.Shutdown();
        }

        protected override void OnResume()
        {
            var cache = Locator.Current.GetService<ICache>();
            cache.Initialize(CacheKey);
        }
    }
}
