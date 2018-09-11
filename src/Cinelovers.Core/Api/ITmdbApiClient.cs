using Cinelovers.Core.Api.Models;
using Refit;
using System;

namespace Cinelovers.Core.Api
{
    public interface ITmdbApiClient
    {
        [Get("/movie/upcoming?page={page}&language={language}")]
        IObservable<MovieResponse> GetUpcomingMovies(int page, string language);

        [Get("/search/movie?query={query}&page={page}&language={language}")]
        IObservable<MovieResponse> GetMovies(string query, int page, string language);

        [Get("/genre/movie/list?language={language}")]
        IObservable<GenreResponse> GetGenres(string language);
    }
}
