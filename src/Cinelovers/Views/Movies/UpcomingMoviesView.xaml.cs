using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Cinelovers.Views.Movies
{
    public partial class UpcomingMoviesView : ContentPageBase<UpcomingMoviesViewModel>
    {
        public UpcomingMoviesView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.Movies, x => x.MovieList.ItemsSource).DisposeWith(Disposables);
            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.Indicator.IsLoading).DisposeWith(Disposables);
            this.Bind(ViewModel, x => x.SelectedMovie, x => x.MovieList.SelectedItem).DisposeWith(Disposables);
            this.Bind(ViewModel, x => x.SearchTerm, x => x.Search.Text).DisposeWith(Disposables);
        }

    }
}