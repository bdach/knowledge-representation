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
    /// <summary>
    /// Extension class for <see cref="IFormula"/>
    /// </summary>
    public static class FormulaExtensions
    {
        /// <summary>
        /// Converts a given <see cref="IFormula"/> into <see cref="IDnfFormula"/>
        /// </summary>
        /// <param name="formula">Formula to be converted to DNF</param>
        /// <returns>Instance of <see cref="IDnfFormula"/></returns>
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