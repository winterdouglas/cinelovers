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

            return _cache
                .GetAndFetchLatest(
                    GetUpComingMoviesCacheKey(page),
                    () => _client.FetchUpcomingMovies(page, Language))
                .Select(pagingInfo => pagingInfo == null 
                    ? Enumerable.Empty<Movie>()    
                    : pagingInfo.Results.Select(result => movieMapper.ToMovie(result)));
        }

        public IObservable<IEnumerable<Genre>> GetMovieGenres()
        {
            var genreMapper = new GenreMapper();

            return _cache
                .GetAndFetchLatest(
                    GetGenresCacheKey(),
                    () => _client.FetchMovieGenres(Language))
                .Select(genreInfo => genreInfo == null
                    ? Enumerable.Empty<Genre>()
                    : genreInfo.Genres.Select(result => genreMapper.ToGenre(result)));
        }

        private string GetUpComingMoviesCacheKey(int page) => $"upcoming_movies_{page}";

        private string GetGenresCacheKey() => "genres";
    }
}
