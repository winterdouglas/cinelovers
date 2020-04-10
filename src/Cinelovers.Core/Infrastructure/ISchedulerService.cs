using System.Reactive.Concurrency;

namespace Cinelovers.Core.Infrastructure
{
    public interface ISchedulerService
    {
        IScheduler MainThread { get; }

        IScheduler TaskPool { get; }

        IScheduler CurrentThread { get; }

        IScheduler Immediate { get; }

        IScheduler NewThread { get; }

        IScheduler Default { get; }
    }
}
