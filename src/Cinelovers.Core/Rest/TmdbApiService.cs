using Refit;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Net;
using Cinelovers.Core.Infrastructure;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiService : ITmdbApiService
    {
        static readonly string BaseAddress = "https://api.themoviedb.org/3";
        static readonly string ApiKey = Secrets.TmdbApi;

        public ITmdbApiClient GetClient()
        {
            var clientHandler = new TmdbHttpClientHandler(() => ApiKey)
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(BaseAddress)
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCaseContractResolver()
            };

            return RestService
                .For<ITmdbApiClient>(
                    httpClient,
                    new RefitSettings
                    {
                        ContentSerializer = new JsonContentSerializer(serializerSettings)
                    });
        }
    }
}
