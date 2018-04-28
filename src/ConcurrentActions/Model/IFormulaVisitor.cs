using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace Model
{
    /// <summary>
    /// Implementation of the Visitor pattern for <see cref="IFormula"/>
    /// </summary>
    public interface IFormulaVisitor
    {
        /// <summary>
        /// Visits an instance of <see cref="Conjunction"/>
        /// </summary>
        /// <param name="conjunction">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Conjunction conjunction);

        /// <summary>
        /// Visits an instance of <see cref="Alternative"/>
        /// </summary>
        /// <param name="alternative">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Alternative alternative);

        /// <summary>
        /// Visits an instance of <see cref="Equivalence"/>
        /// </summary>
        /// <param name="equivalence">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Equivalence equivalence);

        /// <summary>
        /// Visits an instance of <see cref="Implication"/>
        /// </summary>
        /// <param name="implication">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Implication implication);

        /// <summary>
        /// Visits an instance of <see cref="Constant"/>
        /// </summary>
        /// <param name="constant">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Constant constant);

        /// <summary>
        /// Visits an instance of <see cref="Literal"/>
        /// </summary>
        /// <param name="literal">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Literal literal);

        /// <summary>
        /// Visits an instance of <see cref="Negation"/>
        /// </summary>
        /// <param name="negation">Formula to be visited</param>
        /// <returns>Result of the visit, can be either a new instance of <see cref="IFormula"/> or the same</returns>
        IFormula Visit(Negation negation);
    }
}
