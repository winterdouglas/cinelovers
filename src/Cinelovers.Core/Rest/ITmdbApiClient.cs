using Cinelovers.Core.Rest.Dtos;
using Refit;
using System;

namespace Cinelovers.Core.Rest
{
    public interface ITmdbApiClient
    {
        [Get("/movie/upcoming?language={language}&page={page}")]
        IObservable<MoviePagingInfo> FetchUpcomingMovies(int page, string language);
    }
}
