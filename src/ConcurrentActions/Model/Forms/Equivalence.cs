namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical equivalence formula.
    /// </summary>
    public class Equivalence : IFormula
    {
        /// <summary>
        /// The left <see cref="IFormula"/> instance.
        /// </summary>
        public IFormula Left { get; set; }

        /// <summary>
        /// The right <see cref="IFormula"/> instance.
        /// </summary>
        public IFormula Right { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Equivalence"/> instance, representing a logical equivalence of the supplied <see cref="IFormula"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="IFormula"/> instance.</param>
        /// <param name="right">The right <see cref="IFormula"/> instance.</param>
        public Equivalence(IFormula left, IFormula right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return !(Left.Evaluate(state) ^ Right.Evaluate(state));
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"({Left} \u2261 {Right})";
        }

        protected bool Equals(Equivalence other)
        {
            return Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Equivalence) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Left != null ? Left.GetHashCode() : 0) * 397) ^ (Right != null ? Right.GetHashCode() : 0);
            }
        }
    }
}