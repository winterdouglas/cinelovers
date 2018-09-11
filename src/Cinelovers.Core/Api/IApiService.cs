namespace Cinelovers.Core.Api
{
    public interface IApiService
    {
        ITmdbApiClient GetClient();
    }
}