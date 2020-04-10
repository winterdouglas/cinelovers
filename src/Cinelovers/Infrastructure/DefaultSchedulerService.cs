using Cinelovers.Core.Infrastructure;
using ReactiveUI;
using System.Reactive.Concurrency;

namespace Cinelovers
{
    public class DefaultSchedulerService : ISchedulerService
    {
        public IScheduler MainThread => RxApp.MainThreadScheduler;

        public IScheduler TaskPool => TaskPoolScheduler.Default;

        public IScheduler CurrentThread => CurrentThreadScheduler.Instance;

        public IScheduler Immediate => ImmediateScheduler.Instance;

        public IScheduler NewThread => NewThreadScheduler.Default;

        public IScheduler Default => DefaultScheduler.Instance;
    }
}
