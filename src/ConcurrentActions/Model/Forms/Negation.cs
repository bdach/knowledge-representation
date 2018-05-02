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

        public override string ToString()
        {
            return $"\u00AC({Formula})";
        }
    }
}