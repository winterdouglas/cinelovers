﻿using System;
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
        const string ApiKey = "1f54bd990f1cdfb230adb312546d765d";

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
                    () => _client.FetchUpcomingMovies(ApiKey, page, Language))
                .Select(pagingInfo => pagingInfo == null 
                    ? Enumerable.Empty<Movie>()    
                    : pagingInfo.Results.Select(result => movieMapper.ToMovie(result)));
        }

        private string GetUpComingMoviesCacheKey(int page) => $"upcoming_movies_{page}";
    }
}
