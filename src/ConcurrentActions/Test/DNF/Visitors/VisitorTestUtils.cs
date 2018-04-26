using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;
using Moq;

namespace Test.DNF.Visitors
{
    internal static class VisitorTestUtils
    {
        internal static (IFormula, IFormula) MockFormulaAndAcceptResult(IFormulaVisitor visitor)
        {
            var mock = new Mock<IFormula>();
            var result = new Mock<IFormula>();
            mock.Setup(s => s.Accept(visitor)).Returns(result.Object);
            return (mock.Object, result.Object);
        }
    }
}