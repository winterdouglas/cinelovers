using System.Collections.Generic;

namespace Cinelovers.Core.Rest.Dtos
{
    public class MoviePagingInfo
    {
        public int Page { get; set; }
        public IList<MovieListResult> Results { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
    }
}
