using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF.Visitors;
using Model;
using Model.Forms;

namespace DynamicSystem.DNF
{
    public static class FormulaExtensions
    {
        public static IDnfFormula ToDnf(this IFormula formula)
        {
            var dnfFormula = formula
                .Accept(new SimplifyingFormulaVisitor())
                .Accept(new NegationPropagatingFormulaVisitor())
                .Accept(new AlternativeDistributionFormulaVisitor());

            var conjunctions = new List<NaryConjunction>();

            dnfFormula.Accept(new NaryConjunctionGeneratingFormulaVisitor(conjunctions.Add));
            return new DnfFormula(dnfFormula, conjunctions);
        }
    }
}