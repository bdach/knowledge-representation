using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;

namespace DynamicSystem.DNF.Visitors
{
    /// <summary>
    /// Implementation of <see cref="IFormulaVisitor"/> that converts conjunctions of alternatives into alternatives of conjunctions
    /// using the Distributive Law.
    /// </summary>
    internal class AlternativeDistributionFormulaVisitor : IFormulaVisitor
    {
        /// <inheritdoc />
        public IFormula Visit(Conjunction conjunction)
        {
            var rotatedL = conjunction.Left.Accept(this);
            var rotatedR = conjunction.Right.Accept(this);
            if (rotatedL is Alternative || rotatedR is Alternative)
            {
                var disjunction = rotatedL is Alternative ? rotatedL as Alternative : rotatedR as Alternative;
                var other = disjunction == rotatedL ? rotatedR : rotatedL;
                return new Alternative(
                    new Conjunction(other, disjunction.Left).Accept(this),
                    new Conjunction(other, disjunction.Right).Accept(this)
                );
            }
            else
            {
                return new Conjunction(rotatedL, rotatedR);
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Alternative alternative)
        {
            return new Alternative(alternative.Left.Accept(this), alternative.Right.Accept(this));
        }

        /// <inheritdoc />
        public IFormula Visit(Equivalence equivalence)
        {
            throw new InvalidOperationException("equivalence not expected");
        }

        /// <inheritdoc />
        public IFormula Visit(Implication implication)
        {
            throw new InvalidOperationException("implication not expected");
        }

        /// <inheritdoc />
        public IFormula Visit(Constant constant)
        {
            return constant;
        }

        /// <inheritdoc />
        public IFormula Visit(Literal literal)
        {
            return literal;
        }

        /// <inheritdoc />
        public IFormula Visit(Negation negation)
        {
            throw new InvalidOperationException("negation not expected");
        }
    }
}