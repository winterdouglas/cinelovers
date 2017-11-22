using Refit;
using Newtonsoft.Json;
using SnakeCase.JsonNet;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiService : ITmdbApiService
    {
        const string BaseAddress = "https://api.themoviedb.org/3";

        public ITmdbApiClient GetClient() => RestService
            .For<ITmdbApiClient>(
                BaseAddress,
                new RefitSettings
                {
                    JsonSerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new SnakeCaseContractResolver()
                    }
                });
    }
}
