namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical alternative of two formulae.
    /// </summary>
    public class Alternative : IFormula
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
        /// Initializes a new <see cref="Alternative"/> instance, representing a logical conjunction of the supplied <see cref="IFormula"/> instances.
        /// </summary>
        /// <param name="left">The left <see cref="IFormula"/> instance</param>
        /// <param name="right">The right <see cref="IFormula"/> instance.</param>
        public Alternative(IFormula left, IFormula right)
        {
            Left = left;
            Right = right;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return Left.Evaluate(state) || Right.Evaluate(state);
        }
    }
}