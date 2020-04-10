using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Cinelovers.Droid.Renderers;
using FFImageLoading;
using Android.Content;

[assembly: ExportRenderer(typeof(CollectionView), typeof(PauseLoadingImagesCollectionViewRenderer))]

namespace Cinelovers.Droid.Renderers
{
    public class PauseLoadingImagesCollectionViewRenderer : CollectionViewRenderer
    {
        public PauseLoadingImagesCollectionViewRenderer(Context context)
            : base(context)
        {
        }

        public override void OnScrollStateChanged(int state)
        {
            switch (state)
            {
                case ScrollStateDragging:
                    ImageService.Instance.SetPauseWork(true);
                    break;

                case ScrollStateIdle:
                    ImageService.Instance.SetPauseWork(false);
                    break;
            }

            base.OnScrollStateChanged(state);
        }
    }
}