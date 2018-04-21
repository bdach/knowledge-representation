namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical conjunction of two formulae.
    /// </summary>
    public class Conjunction : IFormula
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
        /// Initializes a new <see cref="Conjunction"/> instance, representing a logical conjunction of the supplied <see cref="IFormula"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="IFormula"/> instance.</param>
        /// <param name="right">The right <see cref="IFormula"/> instance.</param>
        public Conjunction(IFormula left, IFormula right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return Left.Evaluate(state) && Right.Evaluate(state);
        }

        public override string ToString()
        {
            return $"({Left} \u2227 {Right})";
        }
    }
}