using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;

namespace Cinelovers.Views.Movies
{
    public partial class UpcomingMoviesView : ReactiveContentPage<UpcomingMoviesViewModel>
    {
        public UpcomingMoviesView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, x => x.Movies, x => x.MovieList.ItemsSource);
            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.Indicator.IsLoading);
            this.Bind(ViewModel, x => x.SelectedMovie, x => x.MovieList.SelectedItem);
            this.Bind(ViewModel, x => x.SearchTerm, x => x.Search.Text);
        }
    }
}