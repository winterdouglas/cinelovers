using System.Net.Http;
using Cinelovers.Core.Infrastructure;
using Cinelovers.Core.Rest;
using Fusillade;

namespace Cinelovers.iOS.Infrastructure
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(Priority priority)
        {
            var clientHandler = new NSUrlSessionHandler();
            var apiHandler = new TmdbApiHandler(clientHandler, () => Secrets.TmdbApi);
            var rateHandler = new RateLimitedHttpMessageHandler(apiHandler, priority);
            return new HttpClient(rateHandler);
        }
    }
}
