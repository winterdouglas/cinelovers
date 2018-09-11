using System;
using Cinelovers.Core.Services.Models;
using Splat;
using System.Reactive.Linq;
using Cinelovers.Core.Mappers;
using DynamicData;
using System.Reactive;
using Cinelovers.Core.Api;
using Cinelovers.Core.Api.Models;
using Cinelovers.Infrastructure.Caching;

namespace Cinelovers.Core.Services
{
    public class MovieService : IMovieService
    {
        public IObservableCache<Movie, int> Movies => _moviesCache;

        public IObservableCache<Movie, int> UpcomingMovies => _upcomingMoviesCache;

        private readonly SourceCache<Movie, int> _moviesCache;
        private readonly SourceCache<Movie, int> _upcomingMoviesCache;
        private readonly MovieMapper _movieMapper;
        private readonly IApiService _apiService;
        private readonly ITmdbApiClient _client;
        private readonly ICache _cache;

        const string Language = "en-US";

        public MovieService(IApiService apiService = null, ICache cache = null)
        {
            _apiService = apiService ?? Locator.Current.GetService<IApiService>();
            _cache = cache ?? Locator.Current.GetService<ICache>();

            _client = _apiService.GetClient();
            _moviesCache = new SourceCache<Movie, int>(m => m.Id);
            _upcomingMoviesCache = new SourceCache<Movie, int>(m => m.Id);

            _movieMapper = new MovieMapper();
        }

        public IObservable<Unit> GetUpcomingMovies(int page)
        {
            return GetAndFetchUpcomingMovies(page).CombineLatest(
                GetAndFetchGenres(),
                (movieResponse, genreResponse) =>
                {
                    var movies = _movieMapper.Map(movieResponse, genreResponse);
                    _upcomingMoviesCache.Edit(cache =>
                    {
                        if (page <= 1)
                            cache.Clear();

                        cache.AddOrUpdate(movies);
                    });
                    return Unit.Default;
                });
        }

        public IObservable<Unit> GetMovies(string query, int page)
        {
            return GetAndFetchMovies(query, page).CombineLatest(
                GetAndFetchGenres(),
                (movieResponse, genreResponse) =>
                {
                    var movies = _movieMapper.Map(movieResponse, genreResponse);
                    _moviesCache.Edit(cache =>
                    {
                        if (page <= 1)
                            cache.Clear();

                        cache.AddOrUpdate(movies);
                    });
                    return Unit.Default;
                });
        }

        private IObservable<MovieResponse> GetAndFetchUpcomingMovies(int page)
        {
            return _cache.GetAndFetchLatest(
                GetUpcomingMoviesCacheKey(page),
                () => _client.GetUpcomingMovies(page, Language));
        }

        private IObservable<MovieResponse> GetAndFetchMovies(string query, int page)
        {
            return _cache.GetAndFetchLatest(
                GetMoviesCacheKey(query, page),
                () => _client.GetMovies(query, page, Language));
        }

        private IObservable<GenreResponse> GetAndFetchGenres()
        {
            return _cache.GetAndFetchLatest(
                GetGenresCacheKey(),
                () => _client.GetGenres(Language));
        }

        private string GetUpcomingMoviesCacheKey(int page) => $"upcoming_movies_{page}";

        private string GetMoviesCacheKey(string query, int page) => $"movies_{query}_{page}";

        private string GetGenresCacheKey() => "genres";
    }
}
