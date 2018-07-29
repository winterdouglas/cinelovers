using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinelovers.Core.Services.Models
{
    public class Movie : EntityBase, IEquatable<Movie>
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

        public bool Equals(Movie other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equals = Id == other.Id
                && Title == other.Title
                && Overview == other.Overview
                && LargePosterUri == other.LargePosterUri
                && SmallPosterUri == other.SmallPosterUri
                && ReleaseDate == other.ReleaseDate
                && Popularity == other.Popularity
                && VoteCount == other.VoteCount
                && VoteAverage == other.VoteAverage
                && Genres.Count == other.Genres.Count
                && Genres.All(other.Genres.Contains);
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Movie)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = -192458408;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Overview);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(LargePosterUri);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(SmallPosterUri);
            hashCode = hashCode * -1521134295 + EqualityComparer<IList<Genre>>.Default.GetHashCode(Genres);
            hashCode = hashCode * -1521134295 + EqualityComparer<DateTime?>.Default.GetHashCode(ReleaseDate);
            hashCode = hashCode * -1521134295 + Popularity.GetHashCode();
            hashCode = hashCode * -1521134295 + VoteCount.GetHashCode();
            hashCode = hashCode * -1521134295 + VoteAverage.GetHashCode();
            return hashCode;
        }
    }
}
