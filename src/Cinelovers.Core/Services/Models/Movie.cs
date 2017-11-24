using System;
using System.Collections.Generic;

namespace Cinelovers.Core.Services.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Overview { get; set; }

        public string PosterUrl { get; set; }

        public string BackdropUrl { get; set; }

        public IList<string> Genres { get; set; }

        public DateTime ReleaseDate { get; set; }

        public double Popularity { get; set; }

        public int VoteCount { get; set; }

        public double VoteAverage { get; set; }
    }
}
