using Cinelovers.ViewModels.Movies;
using ReactiveUI;
using ReactiveUI.XamForms;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Collections.Generic;

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
                this.OneWayBind(ViewModel, x => x.IsLoading, x => x.LoadingIndicator.IsRunning).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SelectedMovie, x => x.MovieList.SelectedItem).DisposeWith(disposables);
                this.Bind(ViewModel, x => x.SearchTerm, x => x.Search.Text).DisposeWith(disposables);

                var nextPageResqueted = Observable
                    .FromEventPattern<ItemVisibilityEventArgs>(
                        x => MovieList.ItemAppearing += x,
                        x => MovieList.ItemAppearing -= x)
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .SelectMany(ev => GetNextPage(ViewModel.Movies, ev.EventArgs.Item as MovieCellViewModel))
                    .Publish();

                nextPageResqueted
                    .Where(page => string.IsNullOrWhiteSpace(ViewModel.SearchTerm))
                    .StartWith(1)
                    .DistinctUntilChanged()
                    .InvokeCommand(ViewModel, x => x.GetUpcomingMovies)
                    .DisposeWith(disposables);

                nextPageResqueted
                    .Where(page => !string.IsNullOrWhiteSpace(ViewModel.SearchTerm))
                    .DistinctUntilChanged()
                    .InvokeCommand(ViewModel, x => x.GetMovies)
                    .DisposeWith(disposables);

                nextPageResqueted
                    .Connect()
                    .DisposeWith(disposables);
            });
        }

        private IObservable<int> GetNextPage<T>(IList<T> items, T currentItem, int pageSize = 20)
        {
            return Observable.Create<int>(
                observer =>
                {
                    var lastPage = (int)Math.Ceiling((double)items.Count / pageSize);
                    var position = items.IndexOf(currentItem) + 1;
                    var nextPage = position % PageSize == 0 ? ++lastPage : lastPage;

                    observer.OnNext(nextPage);
                    observer.OnCompleted();

                    return Disposable.Empty;
                });
        }
    }
}