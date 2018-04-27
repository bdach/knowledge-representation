using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace DynamicSystem.DNF
{
    public class NaryConjunction
    {
        public List<Constant> Constants { get; }
        public List<Literal> Literals { get; }

        public NaryConjunction(List<Literal> literals, List<Constant> constants)
        {
            Literals = literals;
            Constants = constants;
        }

        public bool Conflicts(NaryConjunction other)
        {
            return Constants.Any(c => c.Equals(Constant.Falsity))
                   || other.Constants.Any(c => c.Equals(Constant.Falsity))
                   || Conflict(Literals, other.Literals);
        }

        private static bool Conflict(List<Literal> first, List<Literal> second)
        {
            return first.Any(a => second.Any(b => a.Fluent.Equals(b.Fluent) && a.Negated != b.Negated));
        }


        protected bool Equals(NaryConjunction other)
        {
            return Enumerable.SequenceEqual(Constants, other.Constants) &&
                   Enumerable.SequenceEqual(Literals, other.Literals);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NaryConjunction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Constants != null ? Constants.GetHashCode() : 0) * 397) ^
                       (Literals != null ? Literals.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            var constants = Constants.Count > 0
                ? Constants.Select(c => c.ToString()).Aggregate((a, b) => $"{a}, {b}")
                : "";
            var literals = Literals.Count > 0
                ? Literals.Select(c => c.ToString()).Aggregate((a, b) => $"{a}, {b}")
                : "";
            return $"[{constants}], [{literals}]";
        }
    }
}