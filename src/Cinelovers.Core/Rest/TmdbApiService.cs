using Refit;
using Newtonsoft.Json;
using SnakeCase.JsonNet;

namespace Cinelovers.Core.Rest
{
    public class TmdbApiService
    {
        public string BaseAddress => "https://api.themoviedb.org/3";

        public ITmdbApiClient Client => RestService
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
