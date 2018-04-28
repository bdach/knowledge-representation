namespace Model.Forms
{
    /// <summary>
    /// Base interface for all logical formulae in the data model.
    /// </summary>
    public interface IFormula
    {
        /// <summary>
        /// Evaluates and returns the formula's logical value in the supplied fluent <see cref="State"/>.
        /// </summary>
        /// <param name="state">A <see cref="State"/> object describing the values of all fluents in the formula.</param>
        /// <returns>Boolean value of the formula in the supplied <see cref="state"/>.</returns>
        bool Evaluate(IState state);
    }
}