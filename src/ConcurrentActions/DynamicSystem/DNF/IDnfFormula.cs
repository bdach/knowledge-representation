using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace DynamicSystem.DNF
{
    /// <summary>
    /// Interface representing a formula in disjunctive normal form (DNF)
    /// </summary>
    public interface IDnfFormula : IFormula
    {
        /// <summary>
        /// The conjunctions that the formula consists of
        /// </summary>
        List<NaryConjunction> Conjunctions { get; }

        /// <summary>
        /// Checks whether this formula conflicts with an other one.
        /// </summary>
        /// <param name="other">The formula with which the conflict is checked</param>
        /// <returns>True if formulas are conflicting, else False</returns>
        bool Conflicts(IDnfFormula other);
    }
}