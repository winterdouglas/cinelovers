using Cinelovers.Core.Rest.Dtos;
using Cinelovers.Core.Services.Models;
using System;

namespace Cinelovers.Core.Mappers
{
    public class GenreMapper
    {
        public Genre ToGenre(GenreResult source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new Genre()
            {
                Id = source.Id,
                Name = source.Name
            };
        }
    }
}
