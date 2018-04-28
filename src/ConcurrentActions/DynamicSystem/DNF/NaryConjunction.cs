using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace DynamicSystem.DNF
{
    /// <summary>
    /// Represents a n-ary logical conjunction of <see cref="Literal"/> and <see cref="Constant"/>
    /// </summary>
    public class NaryConjunction
    {
        public List<Constant> Constants { get; }
        public List<Literal> Literals { get; }

        /// <summary>
        /// Initializes a new <see cref="NaryConjunction"/> instance
        /// </summary>
        /// <param name="literals">List of operands that are literals</param>
        /// <param name="constants">List of constant operands</param>
        public NaryConjunction(List<Literal> literals, List<Constant> constants)
        {
            Literals = literals;
            Constants = constants;
        }

        /// <summary>
        /// Checks whether this instance conflicts with another. Two instances conflcit with each other if at least one of the following conditions is true:
        /// 1. Any of the instances contain Falsity constant
        /// 2. Instances contain conflicting literals - with same fluent but one negated and the other not.
        /// </summary>
        /// <param name="other">Instance of <see cref="NaryConjunction"/> with which the conflict is checked</param>
        /// <returns>True if conjunctions are conflicting, else False</returns>
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