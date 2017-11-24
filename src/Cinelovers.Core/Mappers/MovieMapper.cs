using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Services.Models;
using System;

namespace Cinelovers.Core.Mappers
{
    public sealed class MovieMapper
    {
        public Movie ToMovie(MovieResult source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var result = new Movie()
            {
                BackdropUrl = source.BackdropPath,
                //Genres = source.GenreIds,
                Id = source.Id,
                Overview = source.Overview,
                Popularity = source.Popularity,
                PosterUrl = source.PosterPath,
                //ReleaseDate = source.ReleaseDate,
                Title = source.Title,
                VoteAverage = source.VoteAverage,
                VoteCount = source.VoteCount
            };

            return result;
        }
    }
}
