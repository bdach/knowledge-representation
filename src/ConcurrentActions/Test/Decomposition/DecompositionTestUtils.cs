using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;

namespace Test.Decomposition
{
    internal static class DecompositionTestUtils
    {
        public static IFormula CreateBinaryConjunction(string nameA, string nameB, bool negatedA = false, bool negatedB = false)
        {
            return new Conjunction(new Literal(new Fluent(nameA), negatedA), new Literal(new Fluent(nameB), negatedB));
        }
    }
}
