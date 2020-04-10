using System.Net.Http;
using Fusillade;

namespace Cinelovers.Core.Infrastructure
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(Priority priority);
    }
}
