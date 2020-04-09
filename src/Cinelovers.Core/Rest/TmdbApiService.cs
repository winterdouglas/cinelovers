using Refit;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using Cinelovers.Core.Infrastructure;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiService : ITmdbApiService
    {
        static readonly string BaseAddress = "https://api.themoviedb.org/3";
        private readonly HttpClient _httpClient;

        public TmdbApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public ITmdbApiClient GetApiClient()
        {
            _httpClient.BaseAddress = new Uri(BaseAddress);

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCaseContractResolver()
            };

            return RestService
                .For<ITmdbApiClient>(
                    _httpClient,
                    new RefitSettings
                    {
                        ContentSerializer = new NewtonsoftJsonContentSerializer(serializerSettings)
                    });
        }
    }
}
