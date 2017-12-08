using System.Collections.Generic;

namespace Cinelovers.Core.Rest.Dtos
{
    public class GenreInfo
    {
        public IList<GenreResult> Genres { get; set; }

        public GenreInfo()
        {
            Genres = new List<GenreResult>();
        }
    }
}
