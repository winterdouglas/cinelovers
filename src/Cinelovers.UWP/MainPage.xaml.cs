using FFImageLoading.Forms.Platform;

namespace Cinelovers.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            CachedImageRenderer.Init();

            LoadApplication(new Cinelovers.App());
        }
    }
}
