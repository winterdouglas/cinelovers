using System;
using System.Collections.Generic;

namespace Cinelovers.Core.Services.Models
{
    public class Genre : EntityBase, IEquatable<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Equals(Genre other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var equals = Id == other.Id && Name == other.Name;
            return equals;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Genre)obj);
        }

        public override int GetHashCode()
        {
            var hashCode = -1488479220;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
