using System;
using System.Net;
using System.Net.Http;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Rest;
using Fusillade;
using Xamarin.Android.Net;

namespace Cinelovers.Droid.Infrastructure
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(Priority priority)
        {
            var clientHandler = new AndroidClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var apiHandler = new TmdbApiHandler(clientHandler, () => Secrets.TmdbApi);
            var rateHandler = new RateLimitedHttpMessageHandler(apiHandler, priority);
            return new HttpClient(rateHandler);
        }

        public HttpClient CreateClient(Priority priority, string baseUri)
        {
            var client = CreateClient(priority);
            client.BaseAddress = new Uri(baseUri);
            return client;
        }

        public HttpClient CreateClient(Priority priority, Uri baseUri)
        {
            var client = CreateClient(priority);
            client.BaseAddress = baseUri;
            return client;
        }
    }
}
