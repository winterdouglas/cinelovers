using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinelovers.Core.Mappers
{
    public sealed class MovieMapper
    {
        public Movie ToMovie(MovieResult source, GenreInfo genreInfo)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (genreInfo == null) throw new ArgumentNullException(nameof(genreInfo));

            var genres = GetGenres(source, genreInfo);
            var releaseDate = ParseReleaseDate(source);

            var result = new Movie()
            {
                BackdropUrl = source.BackdropPath,
                Id = source.Id,
                Overview = source.Overview,
                Popularity = source.Popularity,
                PosterUrl = source.PosterPath,
                Title = source.Title,
                VoteAverage = source.VoteAverage,
                VoteCount = source.VoteCount,
                Genres = genres,
                ReleaseDate = releaseDate
            };

            return result;
        }

        private DateTime? ParseReleaseDate(MovieResult source)
        {
            DateTime? releaseDate = null;
            if (!string.IsNullOrEmpty(source.ReleaseDate))
            {
                releaseDate = DateTime.Parse(source.ReleaseDate);
            }
            return releaseDate;
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
