using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace Model
{
    public interface IFormulaVisitor
    {
        IFormula Visit(Conjunction conjunction);
        IFormula Visit(Alternative alternative);
        IFormula Visit(Equivalence equivalence);
        IFormula Visit(Implication implication);
        IFormula Visit(Constant constant);
        IFormula Visit(Literal literal);
        IFormula Visit(Negation negation);
    }
}
