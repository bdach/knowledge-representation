using System.Collections.Generic;
using System.Linq;

namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical negation of a formula.
    /// </summary>
    public class Negation : IFormula
    {
        /// <summary>
        /// The <see cref="IFormula"/> to negate.
        /// </summary>
        public IFormula Formula { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Negation() { }

        /// <summary>
        /// Initializes a new <see cref="Negation"/> instance.
        /// </summary>
        /// <param name="formula">The <see cref="IFormula"/> whose value should be negated.</param>
        public Negation(IFormula formula)
        {
            Formula = formula;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return !Formula.Evaluate(state);
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public IEnumerable<Fluent> Fluents => Formula.Fluents;

        public override string ToString()
        {
            return $"\u00AC({Formula})";
        }

        protected bool Equals(Negation other)
        {
            return Equals(Formula, other.Formula);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Negation) obj);
        }

        public override int GetHashCode()
        {
            return (Formula != null ? Formula.GetHashCode() : 0);
        }
    }
}