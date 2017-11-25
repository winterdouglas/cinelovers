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
        const int PageSize = 20;

        public UpcomingMoviesView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Movies, x => x.MovieList.ItemsSource).DisposeWith(disposables);

                //Observable
                //    .FromEventPattern<ItemVisibilityEventArgs>(
                //        x => MovieList.ItemAppearing += x,
                //        x => MovieList.ItemAppearing -= x)
            });
        }
    }
}