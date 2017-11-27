using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Globalization;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace Cinelovers.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieCellView : ReactiveViewCell<MovieCellViewModel>
    {
        public MovieCellView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Title, x => x.TitleLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.ReleasedIn, x => x.ReleaseDateLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.GenresText, x => x.GenreLabel.Text).DisposeWith(disposables);
            });
        }

        /// <summary>
        /// Using the binding context changed instead of binding accordingly to this:
        /// https://github.com/luberda-molinet/FFImageLoading/wiki/Xamarin.Forms-Advanced
        /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/performance/#RecycleElement
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            PosterImage.Source = null;

            var item = BindingContext as MovieCellViewModel;
            if (item == null)
                return;

            PosterImage.Source = item.SmallPosterUri;
        }
    }
}