using Cinelovers.Core.Infrastructure;
using Prism.Navigation;
using ReactiveUI;

namespace Cinelovers.ViewModels.Movies
{
    public class MovieDetailsViewModel : ViewModelBase
    {
        public MovieCellViewModel Movie
        {
            get { return _movie; }
            protected set { this.RaiseAndSetIfChanged(ref _movie, value); }
        }

        private MovieCellViewModel _movie;

        public MovieDetailsViewModel(INavigationService navigationService, ISchedulerService schedulerService)
            : base(navigationService, schedulerService)
        {
        }
    }
}
