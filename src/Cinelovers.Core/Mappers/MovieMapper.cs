using Cinelovers.Core.Rest.Dtos;
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
        const string SmallPosterSize = "w154";
        const string LargePosterSize = "w500";
        const string DefaultCulture = "en-US";

        public Movie ToMovie(MovieResult source, GenreInfo genreInfo)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (genreInfo == null) throw new ArgumentNullException(nameof(genreInfo));

            var genres = GetGenres(source, genreInfo);
            var result = new Movie()
            {
                Id = source.Id,
                Overview = source.Overview,
                SmallPosterUri = new Uri($"{PosterBaseUrl}{SmallPosterSize}{source.PosterPath}", UriKind.Absolute),
                LargePosterUri = new Uri($"{PosterBaseUrl}{LargePosterSize}{source.PosterPath}", UriKind.Absolute),
                Popularity = source.Popularity,
                Title = source.Title,
                VoteAverage = source.VoteAverage,
                VoteCount = source.VoteCount,
                Genres = genres,
                ReleaseDate = DateTime.Parse(
                    source.ReleaseDate, 
                    new CultureInfo(DefaultCulture), 
                    DateTimeStyles.AssumeUniversal)
            };

            return result;
        }

        private IList<Genre> GetGenres(MovieResult source, GenreInfo genreInfo)
        {
            var genreMapper = new GenreMapper();
            return genreInfo
                .Genres
                .Where(list => source.GenreIds.Contains(list.Id))
                .Select(genre => genreMapper.ToGenre(genre))
                .ToList();
        }
    }
}
