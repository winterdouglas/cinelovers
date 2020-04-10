using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;

namespace Cinelovers.Views.Movies
{
    public partial class MovieView : ReactiveContentView<MovieCellViewModel>
    {
        public MovieView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Title, x => x.TitleLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.ReleasedIn, x => x.ReleaseDateLabel.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.GenresText, x => x.GenreLabel.Text).DisposeWith(disposables);
            });
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            PosterImage.Source = null;

            if (BindingContext is MovieCellViewModel item)
            {
                PosterImage.Source = item.SmallPosterUri;
            }
        }
    }
}