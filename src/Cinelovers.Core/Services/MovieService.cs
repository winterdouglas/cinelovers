using System;
using Cinelovers.Core.Services.Models;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Caching;
using Splat;
using System.Reactive.Linq;
using System.Linq;
using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Mappers;
using System.Collections.Generic;

namespace Cinelovers.Core.Services
{
    public class MovieService : IMovieService
    {
        const string Language = "en-US";

        private readonly ITmdbApiService _apiService;
        private readonly ITmdbApiClient _client;
        private readonly ICache _cache;

        public MovieService(
            ITmdbApiService apiService = null,
            ICache cache = null)
        {
            _apiService = apiService ?? Locator.Current.GetService<ITmdbApiService>();
            _cache = cache ?? Locator.Current.GetService<ICache>();

            _client = _apiService.GetClient();
        }

        public IObservable<IEnumerable<Movie>> GetUpcomingMovies(int page)
        {
            var movieMapper = new MovieMapper();

            return Observable
                .CombineLatest(
                    GetAndFetchUpcomingMovies(page),
                    GetAndFetchGenres(),
                    (movieInfo, genreInfo) =>
                    {
                        var result = Enumerable.Empty<Movie>();
                        if (movieInfo != null)
                        {
                            result = movieInfo
                                .Results
                                .Select(source => movieMapper
                                    .ToMovie(source, genreInfo));
                        }
                        return result;
                    });
        }

        public IObservable<IEnumerable<Movie>> GetMovies(string query, int page)
        {
            var movieMapper = new MovieMapper();

            return Observable
                .CombineLatest(
                    GetAndFetchMovies(query, page),
                    GetAndFetchGenres(),
                    (movieInfo, genreInfo) =>
                    {
                        var result = Enumerable.Empty<Movie>();
                        if (movieInfo != null)
                        {
                            result = movieInfo
                                .Results
                                .Select(source => movieMapper
                                    .ToMovie(source, genreInfo));
                        }
                        return result;
                    });
        }

        public IObservable<IEnumerable<Genre>> GetMovieGenres()
        {
            var genreMapper = new GenreMapper();

            return GetAndFetchGenres()
                .Select(genreInfo => genreInfo == null
                    ? Enumerable.Empty<Genre>()
                    : genreInfo.Genres.Select(result => genreMapper.ToGenre(result)));
        }

        private IObservable<MoviePagingInfo> GetAndFetchUpcomingMovies(int page)
        {
            return _cache
                .GetAndFetchLatest(
                    GetUpcomingMoviesCacheKey(page),
                    () => _client.FetchUpcomingMovies(page, Language));
        }

        private IObservable<MoviePagingInfo> GetAndFetchMovies(string query, int page)
        {
            return _cache
                .GetAndFetchLatest(
                    GetMoviesCacheKey(query, page),
                    () => _client.FetchMovies(query, page, Language));
        }

        private IObservable<GenreInfo> GetAndFetchGenres()
        {
            return _cache
                .GetAndFetchLatest(
                    GetGenresCacheKey(),
                    () => _client.FetchMovieGenres(Language));
        }

        private string GetUpcomingMoviesCacheKey(int page) => $"upcoming_movies_{page}";

        private string GetGenresCacheKey() => "genres";

        private string GetMoviesCacheKey(string query, int page) => $"movies_{query}_{page}";
    }
}
