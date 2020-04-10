using System;
using System.Reactive;
using Akavache;

namespace Cinelovers.Core.Caching
{
    public class AkavacheApiCache : IApiCache
    {
        const string ApplicationName = "Cinelovers";

        public AkavacheApiCache()
        {
            Akavache.Sqlite3.Registrations.Start(ApplicationName, () => SQLitePCL.Batteries_V2.Init());
            BlobCache.ForcedDateTimeKind = DateTimeKind.Utc;
        }

        public IObservable<TResult> GetAndFetchLatest<TResult>(string cacheKey, Func<IObservable<TResult>> fetchFunction)
        {
            BlobCache.EnsureInitialized();

            return BlobCache
                .LocalMachine
                .GetAndFetchLatest(
                    cacheKey,
                    fetchFunction,
                    offset => (DateTimeOffset.UtcNow - offset) > TimeSpan.FromHours(6));
        }

        public IObservable<Unit> InvalidateAll()
        {
            BlobCache.EnsureInitialized();

            return BlobCache
                .LocalMachine
                .InvalidateAll();
        }

        public IObservable<Unit> InvalidateAllObjects<T>() where T : class
        {
            BlobCache.EnsureInitialized();

            return BlobCache
                .LocalMachine
                .InvalidateAllObjects<T>();
        }

        public IObservable<Unit> Invalidate(string key)
        {
            BlobCache.EnsureInitialized();

            return BlobCache
                .LocalMachine
                .Invalidate(key);
        }

        public void Shutdown()
        {
            BlobCache
                .Shutdown()
                .Wait();
        }
    }
}