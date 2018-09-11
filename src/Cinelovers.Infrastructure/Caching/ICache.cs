using System;
using System.Reactive;

namespace Cinelovers.Infrastructure.Caching
{
    public interface ICache
    {
        void Initialize(string name);
        IObservable<TResult> GetAndFetchLatest<TResult>(string cacheKey, Func<IObservable<TResult>> fetchFunction);
        IObservable<Unit> InvalidateAll();
        IObservable<Unit> InvalidateAllObjects<T>() where T : class;
        IObservable<Unit> Invalidate(string key);
        void Shutdown();
    }
}
