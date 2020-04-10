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
        private readonly HttpClient _apiHttpClient;

        public TmdbApiService(HttpClient apiHttpClient)
        {
            _apiHttpClient = apiHttpClient ?? throw new ArgumentNullException(nameof(apiHttpClient));
            _apiHttpClient.BaseAddress = new Uri(BaseAddress);
        }

        public ITmdbApiClient GetApiClient()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new SnakeCaseContractResolver()
            };

            return RestService
                .For<ITmdbApiClient>(
                    _apiHttpClient,
                    new RefitSettings
                    {
                        ContentSerializer = new NewtonsoftJsonContentSerializer(serializerSettings)
                    });
        }
    }
}
