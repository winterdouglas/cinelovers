namespace Cinelovers.Core.Rest
{
    public interface IApiService
    {
        ITmdbApiClient GetClient();
    }
}