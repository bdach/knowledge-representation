using System.Collections.Generic;
using System.Xml.Serialization;

namespace Model.Forms
{
    /// <summary>
    /// Base interface for all logical formulae in the data model.
    /// </summary>
    /// <remarks>
    /// <see cref="XmlIncludeAttribute"/> tags should reference all implementors
    /// and inheritors of <see cref="IFormula"/> interface to ensure proper serialization.
    /// </remarks>
    [XmlInclude(typeof(Alternative))]
    [XmlInclude(typeof(Conjunction))]
    [XmlInclude(typeof(Constant))]
    [XmlInclude(typeof(Equivalence))]
    [XmlInclude(typeof(Implication))]
    [XmlInclude(typeof(Literal))]
    [XmlInclude(typeof(Negation))]
    public interface IFormula
    {
        /// <summary>
        /// Evaluates and returns the formula's logical value in the supplied fluent <see cref="State"/>.
        /// </summary>
        /// <param name="state">A <see cref="State"/> object describing the values of all fluents in the formula.</param>
        /// <returns>Boolean value of the formula in the supplied <see cref="state"/>.</returns>
        bool Evaluate(IState state);
        IFormula Accept(IFormulaVisitor visitor);
        IEnumerable<Fluent> Fluents { get; }
    }
}