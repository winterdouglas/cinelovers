using Refit;
using Newtonsoft.Json;
using SnakeCase.JsonNet;
using System.Net.Http;
using System;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiService : ITmdbApiService
    {
        const string BaseAddress = "https://api.themoviedb.org/3";
        const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";

        public ITmdbApiClient GetClient()
        {
            var httpClient = new HttpClient(new TmdbHttpClientHandler(() => ApiKey))
            {
                BaseAddress = new Uri(BaseAddress)
            };

            return RestService
              .For<ITmdbApiClient>(
                  httpClient,
                  new RefitSettings
                  { 
                      JsonSerializerSettings = new JsonSerializerSettings
                      {
                          ContractResolver = new SnakeCaseContractResolver()
                      }
                  });
        }
    }
}
