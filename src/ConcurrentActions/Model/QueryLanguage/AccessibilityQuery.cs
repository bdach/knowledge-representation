using Model.Forms;

namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a single accessibility query.
    /// The target of the query is to answer whether there is a <see cref="Program"/> executable in all models of the domain
    /// that leads to the satisfaction of the associated <see cref="IFormula"/>.
    /// </summary>
    public class AccessibilityQuery
    {
        /// <summary>
        /// The target <see cref="IFormula"/> to be satisfied.
        /// </summary>
        public IFormula Target { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessibilityQuery"/> class.
        /// </summary>
        /// <param name="target">The target <see cref="IFormula"/> to be satisfied.</param>
        public AccessibilityQuery(IFormula target)
        {
            Target = target;
        }

        public override string ToString()
        {
            return $"accessible {Target}";
        }

        protected bool Equals(AccessibilityQuery other)
        {
            return Equals(Target, other.Target);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AccessibilityQuery) obj);
        }

        public override int GetHashCode()
        {
            return (Target != null ? Target.GetHashCode() : 0);
        }
    }
}