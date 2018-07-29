using System;
using System.Collections.Generic;
using System.Text;

namespace Cinelovers.Core.Services.Models
{
    public abstract class EntityBase
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static bool operator ==(EntityBase e1, EntityBase e2)
        {
            if (ReferenceEquals(e1, null))
                return ReferenceEquals(e2, null);

            return e1.Equals(e2);
        }

        public static bool operator !=(EntityBase e1, EntityBase e2)
        {
            return !(e1 == e2);
        }
    }
}
