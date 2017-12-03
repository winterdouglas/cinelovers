namespace Cinelovers.Core.Rest
{
    public interface ITmdbApiService
    {
        ITmdbApiClient GetClient();
    }
}