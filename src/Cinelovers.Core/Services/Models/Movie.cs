using System;
using System.Collections.Generic;

namespace Cinelovers.Core.Services.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Overview { get; set; }

        public Uri LargePosterUri { get; set; }

        public Uri SmallPosterUri { get; set; }

        public IList<Genre> Genres { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public double Popularity { get; set; }

        public int VoteCount { get; set; }

        public double VoteAverage { get; set; }

        public Movie()
        {
            Genres = new List<Genre>();
        }
    }
}
