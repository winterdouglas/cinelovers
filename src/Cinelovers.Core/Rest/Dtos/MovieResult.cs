using System.Collections.Generic;

namespace Cinelovers.Core.Rest.Dtos
{
    public class MovieResult
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Overview { get; set; }

        public string PosterPath { get; set; }

        public string BackdropPath { get; set; }

        public IList<int> GenreIds { get; set; }

        public string ReleaseDate { get; set; }

        public bool Adult { get; set; }

        public double Popularity { get; set; }

        public int VoteCount { get; set; }

        public double VoteAverage { get; set; }
    }
}
