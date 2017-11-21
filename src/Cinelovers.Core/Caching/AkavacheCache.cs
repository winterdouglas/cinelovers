using System;
using System.Reactive;
using Akavache;

namespace Cinelovers.Core.Caching
{
    public class AkavacheCache : ICache
    {
        public void Initialize(string name)
        {
            BlobCache.ApplicationName = name;
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        public IObservable<TResult> GetAndFetchLatest<TResult>(string cacheKey, Func<IObservable<TResult>> fetchFunction)
        {
            return BlobCache.LocalMachine.GetAndFetchLatest(cacheKey, fetchFunction, offset =>
            {
                var ellapsed = DateTimeOffset.Now - offset;
                return ellapsed > TimeSpan.FromSeconds(30);
            });
        }

        public IObservable<Unit> InvalidateAll()
        {
            return BlobCache.LocalMachine.InvalidateAll();
        }

        public IObservable<Unit> InvalidateAllObjects<T>() where T : class
        {
            return BlobCache.LocalMachine.InvalidateAllObjects<T>();
        }

		public IObservable<Unit> Invalidate(string key)
        {
            return BlobCache.LocalMachine.Invalidate(key);
        }

        public void Shutdown()
        {
            BlobCache.Shutdown().Wait();
        }
    }
}