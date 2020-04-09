using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiHandler : DelegatingHandler
    {
        private readonly Func<string> _getApiKey;

        public TmdbApiHandler(HttpMessageHandler innerHandler, Func<string> getApiKey)
            : base(innerHandler)
        {
            _getApiKey = getApiKey ?? throw new ArgumentNullException(nameof(getApiKey));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.RequestUri = GetAuthenticatedUri(request.RequestUri);

            return base.SendAsync(request, cancellationToken);
        }

        private Uri GetAuthenticatedUri(Uri requestUri)
        {
            var apiKey = _getApiKey();
            UriBuilder baseUri = new UriBuilder(requestUri);
            string queryToAppend = $"api_key={apiKey}";

            baseUri.Query = baseUri.Query != null && baseUri.Query.Length > 1
                ? $"{baseUri.Query.Substring(1)}&{queryToAppend}"
                : queryToAppend;

            return baseUri.Uri;
        }
    }
}
