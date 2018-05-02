using System.Collections.Generic;

namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a single literal.
    /// </summary>
    public class Literal : IFormula
    {
        /// <summary>
        /// The <see cref="Model.Fluent"/> with which the literal instance is associated.
        /// </summary>
        public Fluent Fluent { get; set; }

        /// <summary>
        /// Boolean value indicated whether or not the fluent's value should be negated.
        /// </summary>
        public bool Negated { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Literal() { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="Literal" /> instance.
        /// This constructor does not negate the fluent value by default.
        /// </summary>
        public Literal(Fluent fluent) : this(fluent, false) { }

        /// <summary>
        /// Initializes a new <see cref="Literal"/> instance.
        /// </summary>
        /// <param name="fluent">The <see cref="Model.Fluent"/> to be contained in the literal.</param>
        /// <param name="negated">Boolean value, indicating whether or not the literal should be negated.</param>
        public Literal(Fluent fluent, bool negated)
        {
            Fluent = fluent;
            Negated = negated;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return state[Fluent] ^ Negated;
        }

        public override string ToString()
        {
            return Negated ? $"(\u00AC{Fluent})" : Fluent.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Literal literal &&
                   EqualityComparer<Fluent>.Default.Equals(Fluent, literal.Fluent) &&
                   Negated == literal.Negated;
        }

        public override int GetHashCode()
        {
            var hashCode = -2091649536;
            hashCode = hashCode * -1521134295 + EqualityComparer<Fluent>.Default.GetHashCode(Fluent);
            hashCode = hashCode * -1521134295 + Negated.GetHashCode();
            return hashCode;
        }
    }
}