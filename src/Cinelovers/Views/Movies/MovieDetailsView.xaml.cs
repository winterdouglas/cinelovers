using Cinelovers.ViewModels.Movies;
using ReactiveUI.XamForms;
using Xamarin.Forms.Xaml;

namespace Cinelovers.Views.Movies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailsView : ReactiveContentPage<MovieDetailsViewModel>
    {
        public MovieDetailsView()
        {
            InitializeComponent();
        }
    }
}