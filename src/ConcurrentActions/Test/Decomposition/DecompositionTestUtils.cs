using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;

namespace Test.Decomposition
{
    /// <summary>
    /// Static class with useful function for testing decompositions
    /// </summary>
    internal static class DecompositionTestUtils
    {
        /// <summary>
        /// Returns a two element <see cref="Conjunction"/>
        /// </summary>
        /// <param name="nameA">Name of the first fluent</param>
        /// <param name="nameB">Name of the second fluent</param>
        /// <param name="negatedA">Is the first fluent negated?</param>
        /// <param name="negatedB">Is the second fluent negated?</param>
        /// <returns></returns>
        public static IFormula CreateBinaryConjunction(string nameA, string nameB, bool negatedA = false, bool negatedB = false)
        {
            return new Conjunction(new Literal(new Fluent(nameA), negatedA), new Literal(new Fluent(nameB), negatedB));
        }
    }
}
