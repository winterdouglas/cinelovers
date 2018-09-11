using Cinelovers.Core.Api.Models;
using Cinelovers.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Cinelovers.Core.Mappers
{
    public sealed class MovieMapper
    {
        const string PosterBaseUrl = "https://image.tmdb.org/t/p/";
        const string SmallPosterSize = "w185";
        const string LargePosterSize = "w500";
        const string DefaultCulture = "en-US";

        public IList<Movie> Map(MovieResponse movieResponse, GenreResponse genreResponse)
        {
            var result = (from movie in movieResponse.Results
                         select new Movie()
                         {
                             Id = movie.Id,
                             Overview = movie.Overview,
                             SmallPosterUri = new Uri($"{PosterBaseUrl}{SmallPosterSize}{movie.PosterPath}", UriKind.Absolute),
                             LargePosterUri = new Uri($"{PosterBaseUrl}{LargePosterSize}{movie.PosterPath}", UriKind.Absolute),
                             Popularity = movie.Popularity,
                             Title = movie.Title,
                             VoteAverage = movie.VoteAverage,
                             VoteCount = movie.VoteCount,
                             Genres = (from genre in genreResponse.Genres
                                       where movie.GenreIds.Contains(genre.Id)
                                       select new Genre()
                                       {
                                           Id = genre.Id,
                                           Name = genre.Name
                                       }).ToList(),
                             ReleaseDate = ParseDate(movie.ReleaseDate)
                         }).ToList();

            return result;
        }

        private DateTime? ParseDate(string date)
        {
            DateTime? result = null;

            if (!string.IsNullOrEmpty(date))
            {
                result = DateTime.Parse(
                    date,
                    new CultureInfo(DefaultCulture),
                    DateTimeStyles.AssumeUniversal);
            }
            return result;
        }
    }
}
