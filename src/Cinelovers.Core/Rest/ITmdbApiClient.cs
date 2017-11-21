using Cinelovers.Core.Rest.Dtos;
using Refit;
using System;

namespace Cinelovers.Core.Rest
{
    [Headers("Content-Type: application/json")]
    public interface ITmdbApiClient
    {
        [Get("/movie/upcoming?api_key={apiKey}&language={language}&page={page}")]
        IObservable<MoviePagingInfo> FetchUpcomingMovies(string apiKey, int page, string language);
    }
}
