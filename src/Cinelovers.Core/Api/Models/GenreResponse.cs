using System.Collections.Generic;

namespace Cinelovers.Core.Api.Models
{
    public class GenreResponse
    {
        public IList<GenreResult> Genres { get; set; }

        public GenreResponse()
        {
            Genres = new List<GenreResult>();
        }
    }
}
