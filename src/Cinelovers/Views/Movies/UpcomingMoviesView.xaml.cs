using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using Xamarin.Forms.Xaml;

namespace Cinelovers.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingMoviesView : ReactiveContentPage<UpcomingMoviesViewModel>
    {
        public UpcomingMoviesView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Movies, x => x.MovieList.ItemsSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.IsLoading, x => x.Indicator.IsLoading).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SelectedMovie, x => x.MovieList.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SearchTerm, x => x.Search.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, x => x.Load, x => x.MovieList.RemainingItemsThresholdReachedCommand).DisposeWith(disposables);

                ViewModel.SelectedMovie = null;
            });
        }
    }
}