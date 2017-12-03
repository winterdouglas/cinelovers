using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Cinelovers.Droid.Rendereres;
using FFImageLoading;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(ImageListViewRenderer))]
namespace Cinelovers.Droid.Rendereres
{
    public class ImageListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.ScrollStateChanged -= ScrollChanged;
            }
            if (e.NewElement != null)
            {
                Control.ScrollStateChanged += ScrollChanged;
            }
        }

        private void ScrollChanged(object sender, AbsListView.ScrollStateChangedEventArgs e)
        {
            switch (e.ScrollState)
            {
                case ScrollState.Fling:
                    ImageService.Instance.SetPauseWork(true); // all image loading requests will be silently canceled
                    break;
                case ScrollState.Idle:
                    ImageService.Instance.SetPauseWork(false); // loading requests are allowed again
                    break;
                default:
                    break;
            }

            
        }
    }
}