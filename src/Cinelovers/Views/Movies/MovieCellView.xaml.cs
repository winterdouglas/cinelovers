using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Linq;
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
                this.OneWayBind(ViewModel, x => x.Genres, x => x.GenreLabel.Text, 
                    genres => string.Join(", ", genres.Select(x => x.Name))).DisposeWith(disposables);
            });
        }
    }
}