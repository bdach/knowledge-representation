namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing the two distinguished constant formulae.
    /// </summary>
    public class Constant : IFormula
    {
        /// <summary>
        /// The inner constant value of the formula.
        /// </summary>
        private readonly bool _value;

        /// <summary>
        /// Initializes a new <see cref="Constant"/> instance with the supplied <see cref="value"/>.
        /// </summary>
        /// <param name="value">The inner constant value of the formula.</param>
        private Constant(bool value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return _value;
        }

        /// <summary>
        /// The truth constant formula.
        /// </summary>
        public static Constant Truth { get; } = new Constant(true);

        /// <summary>
        /// The falsity constant formula.
        /// </summary>
        public static Constant Falsity { get; } = new Constant(false);

        public override string ToString()
        {
            return _value ? "\u22A4" : "\u22A5";
        }
    }
}