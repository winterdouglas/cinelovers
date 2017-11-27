using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cinelovers.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailsView : ReactiveContentPage<MovieDetailsViewModel>
    {
        public MovieDetailsView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Movie.Title, x => x.TitleLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Movie.ReleasedIn, x => x.ReleaseDateLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Movie.GenresText, x => x.GenreLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Movie.Overview, x => x.OverviewLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Movie.LargePosterUri, x => x.PosterImage.Source,
                    uri => ImageSource.FromUri(uri)).DisposeWith(disposables);
            });
        }
    }
}