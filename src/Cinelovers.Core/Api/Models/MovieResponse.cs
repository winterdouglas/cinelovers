using System.Collections.Generic;

namespace Cinelovers.Core.Api.Models
{
    public class MovieResponse
    {
        public int Page { get; set; }

        public IList<MovieResult> Results { get; set; }

        public int TotalPages { get; set; }

        public int TotalResults { get; set; }

        public MovieResponse()
        {
            Results = new List<MovieResult>();
        }
    }
}
