using System;
using System.Net.Http;
using Fusillade;

namespace Cinelovers.Core.Infrastructure
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(Priority priority);

        HttpClient CreateClient(Priority priority, string baseUri);

        HttpClient CreateClient(Priority priority, Uri baseUri);
    }
}
