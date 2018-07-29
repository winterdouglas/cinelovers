using Cinelovers.Core.Rest.Dtos;
using Refit;
using System;

namespace Cinelovers.Core.Rest
{
    public interface ITmdbApiClient
    {
        [Get("/movie/upcoming?page={page}&language={language}")]
        IObservable<MoviePagingInfo> GetUpcomingMovies(int page, string language);

        [Get("/search/movie?query={query}&page={page}&language={language}")]
        IObservable<MoviePagingInfo> GetMovies(string query, int page, string language);

        [Get("/genre/movie/list?language={language}")]
        IObservable<GenreInfo> GetGenres(string language);
    }
}
