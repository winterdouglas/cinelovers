using System;
using Cinelovers.Core.Services.Models;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Caching;
using Splat;
using System.Reactive.Linq;
using System.Linq;
using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Mappers;
using DynamicData;
using System.Reactive;

namespace Cinelovers.Core.Services
{
    public class MovieService : IMovieService
    {
        public IObservableCache<Movie, int> Movies => _movies;

        public IObservableCache<Genre, int> Genres => _genres;

        private readonly SourceCache<Movie, int> _movies;
        private readonly SourceCache<Genre, int> _genres;
        private readonly IApiService _apiService;
        private readonly ITmdbApiClient _client;
        private readonly ICache _cache;

        const string Language = "en-US";

        public MovieService(
            IApiService apiService = null,
            ICache cache = null)
        {
            _apiService = apiService ?? Locator.Current.GetService<IApiService>();
            _cache = cache ?? Locator.Current.GetService<ICache>();

            _client = _apiService.GetClient();
            _movies = new SourceCache<Movie, int>(m => m.Id);
            _genres = new SourceCache<Genre, int>(g => g.Id);
        }

        public IObservable<Unit> GetUpcomingMovies(int page)
        {
            var movieMapper = new MovieMapper();

            return Observable.CombineLatest(
                GetAndFetchUpcomingMovies(page),
                GetAndFetchGenres(),
                (movieInfo, genreInfo) =>
                {
                    var result = Enumerable.Empty<Movie>();
                    if (movieInfo != null)
                    {
                        result = movieInfo.Results.Select(source => movieMapper.ToMovie(source, genreInfo));
                    }
                    return Unit.Default;
                });
        }

        public IObservable<Unit> GetMovies(string query, int page)
        {
            if (query != null && query.Trim().Length <= 3)
            {
                //TODO: Project empty list
                return Observable.Return(Unit.Default);
            }

            var movieMapper = new MovieMapper();

            return Observable.CombineLatest(
                GetAndFetchMovies(query, page),
                GetAndFetchGenres(),
                (movieInfo, genreInfo) =>
                {
                    var result = Enumerable.Empty<Movie>();
                    if (movieInfo != null)
                    {
                        result = movieInfo.Results.Select(source => movieMapper.ToMovie(source, genreInfo));
                    }
                    return Unit.Default;
                });
        }

        public IObservable<Unit> GetGenres()
        {
            var genreMapper = new GenreMapper();

            return GetAndFetchGenres()
                .Select(genreInfo => genreInfo == null
                    ? Enumerable.Empty<Genre>()
                    : genreInfo.Genres.Select(result => genreMapper.ToGenre(result)))
                .Select(_ => Unit.Default);
        }

        private IObservable<MoviePagingInfo> GetAndFetchUpcomingMovies(int page)
        {
            return _cache.GetAndFetchLatest(
                GetUpcomingMoviesCacheKey(page),
                () => _client.GetUpcomingMovies(page, Language));
        }

        private IObservable<MoviePagingInfo> GetAndFetchMovies(string query, int page)
        {
            return _cache.GetAndFetchLatest(
                GetMoviesCacheKey(query, page),
                () => _client.GetMovies(query, page, Language));
        }

        private IObservable<GenreInfo> GetAndFetchGenres()
        {
            return _cache.GetAndFetchLatest(
                GetGenresCacheKey(),
                () => _client.GetGenres(Language));
        }

        private string GetUpcomingMoviesCacheKey(int page) => $"upcoming_movies_{page}";

        private string GetGenresCacheKey() => "genres";

        private string GetMoviesCacheKey(string query, int page) => $"movies_{query}_{page}";
    }
}
