using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Forms;

namespace DynamicSystem.DNF
{
    public interface IDnfFormula : IFormula
    {
        List<NaryConjunction> Conjunctions { get; }
        bool Conflicts(IDnfFormula other);
    }
}
