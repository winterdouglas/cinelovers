using Cinelovers.Core.Infrastructure;
using Prism.AppModel;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;

namespace Cinelovers.ViewModels
{
    public abstract class ViewModelBase : ReactiveObject, INavigationAware, IDestructible, IInitialize, IPageLifecycleAware
    {
        private readonly ScheduledSubject<INavigationParameters> _initialized;
        private readonly ScheduledSubject<Unit> _appearing;
        private readonly ScheduledSubject<INavigationParameters> _navigatedTo;
        private readonly ScheduledSubject<Unit> _disappearing;
        private readonly ScheduledSubject<INavigationParameters> _navigatedFrom;

        public ViewModelBase(INavigationService navigationService, ISchedulerService schedulerService)
        {
            NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            SchedulerService = schedulerService ?? throw new ArgumentNullException(nameof(schedulerService));
            Disposables = new CompositeDisposable();

            _initialized = new ScheduledSubject<INavigationParameters>(SchedulerService.CurrentThread).DisposeWith(Disposables);
            _appearing = new ScheduledSubject<Unit>(SchedulerService.CurrentThread).DisposeWith(Disposables);
            _navigatedTo = new ScheduledSubject<INavigationParameters>(SchedulerService.CurrentThread).DisposeWith(Disposables);
            _disappearing = new ScheduledSubject<Unit>(SchedulerService.CurrentThread).DisposeWith(Disposables);
            _navigatedFrom = new ScheduledSubject<INavigationParameters>(SchedulerService.CurrentThread).DisposeWith(Disposables);
        }

        public IObservable<INavigationParameters> Initialized => _initialized;

        public IObservable<Unit> Appearing => _appearing;

        public IObservable<INavigationParameters> NavigatedTo => _navigatedTo;

        public IObservable<Unit> Disappearing => _disappearing;

        public IObservable<INavigationParameters> NavigatedFrom => _navigatedFrom;

        protected CompositeDisposable Disposables { get; }

        protected INavigationService NavigationService { get; }

        protected ISchedulerService SchedulerService { get; }

        public void Initialize(INavigationParameters parameters)
        {
            _initialized.OnNext(parameters);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            _navigatedFrom.OnNext(parameters);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            _navigatedTo.OnNext(parameters);
        }

        public void OnAppearing()
        {
            _appearing.OnNext(Unit.Default);
        }

        public void OnDisappearing()
        {
            _disappearing.OnNext(Unit.Default);
        }

        public void Destroy()
        {
            Disposables.Dispose();
        }
    }
}
