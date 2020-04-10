using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Forms;

namespace Cinelovers.Views.Movies
{
    public partial class MovieDetailsView : ContentPageBase<MovieDetailsViewModel>
    {
        public MovieDetailsView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.Movie.Title, x => x.TitleLabel.Text).DisposeWith(Disposables);
            this.OneWayBind(ViewModel, x => x.Movie.ReleasedIn, x => x.ReleaseDateLabel.Text).DisposeWith(Disposables);
            this.OneWayBind(ViewModel, x => x.Movie.GenresText, x => x.GenreLabel.Text).DisposeWith(Disposables);
            this.OneWayBind(ViewModel, x => x.Movie.Overview, x => x.OverviewLabel.Text).DisposeWith(Disposables);
            this.OneWayBind(ViewModel, x => x.Movie.LargePosterUri, x => x.PosterImage.Source, uri => ImageSource.FromUri(uri)).DisposeWith(Disposables);
        }
    }
}