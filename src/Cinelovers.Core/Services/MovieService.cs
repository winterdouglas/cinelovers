using System;
using Cinelovers.Core.Services.Models;
using Cinelovers.Core.Rest;
using Cinelovers.Core.Caching;
using System.Reactive.Linq;
using System.Linq;
using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Mappers;
using System.Collections.Generic;
using System.Reactive;
using DynamicData;

namespace Cinelovers.Core.Services
{
    public class MovieService : IMovieService
    {
        const string Language = "en-US";

        private readonly ITmdbApiClient _apiClient;
        private readonly IApiCache _cache;
        private readonly SourceCache<Movie, int> _movies;
        private readonly MovieMapper _movieMapper;

        public MovieService(ITmdbApiClient apiClient, IApiCache cache)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _movies = new SourceCache<Movie, int>(x => x.Id);
            _movieMapper = new MovieMapper();
        }

        public IObservableCache<Movie, int> Movies => _movies;

        public IObservable<Unit> LoadUpcomingMovies(int page)
        {
            if (page == 1)
            {
                _movies.Clear();
            }

            var movieMapper = new MovieMapper();

            return Observable
                .CombineLatest(
                    GetAndFetchUpcomingMovies(page),
                    GetAndFetchGenres(),
                    (movieInfo, genreInfo) =>
                    {
                        AddOrUpdateMovies(movieInfo, genreInfo);
                        return Unit.Default;
                    });
        }

        public IObservable<Unit> LoadMovies(string query, int page)
        {
            if (page == 1)
            {
                _movies.Clear();
            }

            return Observable
                .CombineLatest(
                    GetAndFetchMovies(query, page),
                    GetAndFetchGenres(),
                    (movieInfo, genreInfo) =>
                    {
                        AddOrUpdateMovies(movieInfo, genreInfo);
                        return Unit.Default;
                    });
        }

        private IObservable<MoviePagingInfo> GetAndFetchUpcomingMovies(int page)
        {
            return _cache
                .GetAndFetchLatest(
                    $"upcoming_movies_{page}",
                    () => _apiClient.FetchUpcomingMovies(page, Language));
        }

        private IObservable<MoviePagingInfo> GetAndFetchMovies(string query, int page)
        {
            return _cache
                .GetAndFetchLatest(
                    $"movies_{query}_{page}",
                    () => _apiClient.FetchMovies(query, page, Language));
        }

        private IObservable<GenreInfo> GetAndFetchGenres()
        {
            return _cache
                .GetAndFetchLatest(
                    "genres",
                    () => _apiClient.FetchMovieGenres(Language));
        }

        private void AddOrUpdateMovies(MoviePagingInfo moviePagingInfo, GenreInfo genreInfo)
        {
            var movies = new List<Movie>();
            if (moviePagingInfo != null && genreInfo != null)
            {
                movies = moviePagingInfo
                    .Results
                    .Select(source => _movieMapper.ToMovie(source, genreInfo))
                    .ToList();
            }

            _movies.AddOrUpdate(movies);
        }
    }
}
