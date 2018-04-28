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
    /// Implementation of <see cref="IFormulaVisitor"/> that removes instances of <see cref="Negation"/> by
    /// propagating them down the formula tree
    /// </summary>
    internal class NegationPropagatingFormulaVisitor : IFormulaVisitor
    {
        private bool Negate { get; set; } = false;

        /// <summary>
        /// Visits a given formula with Negate set to true
        /// </summary>
        /// <param name="formula">Formula to visit</param>
        /// <returns>Result of the visit operation</returns>
        private IFormula NegateAccept(IFormula formula)
        {
            Negate = true;
            return formula.Accept(this);
        }

        /// <inheritdoc />
        public IFormula Visit(Conjunction conjunction)
        {
            if (Negate)
            {
                Negate = false;
                return new Alternative(NegateAccept(conjunction.Left), NegateAccept(conjunction.Right));
            }
            else
            {
                return new Conjunction(conjunction.Left.Accept(this), conjunction.Right.Accept(this));
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Alternative alternative)
        {
            if (Negate)
            {
                Negate = false;
                return new Conjunction(NegateAccept(alternative.Left), NegateAccept(alternative.Right));
            }
            else
            {
                return new Alternative(alternative.Left.Accept(this), alternative.Right.Accept(this));
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Equivalence equivalence)
        {
            if (Negate)
            {
                Negate = false;
                return new Alternative(
                    NegateAccept(new Implication(equivalence.Left, equivalence.Right)),
                    NegateAccept(new Implication(equivalence.Right, equivalence.Left))
                );
            }
            else
            {
                return new Equivalence(equivalence.Left.Accept(this), equivalence.Right.Accept(this));
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Implication implication)
        {
            if (Negate)
            {
                return new Conjunction(implication.Antecedent, NegateAccept(implication.Consequent));
            }
            else
            {
                return new Implication(implication.Antecedent.Accept(this), implication.Consequent.Accept(this));
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Constant constant)
        {
            if (Negate)
            {
                Negate = false;
                return constant.Equals(Constant.Falsity) ? Constant.Truth : Constant.Falsity;
            }
            else
            {
                return constant;
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Literal literal)
        {
            if (Negate)
            {
                Negate = false;
                return new Literal(literal.Fluent, !literal.Negated);
            }
            else
            {
                return literal;
            }
        }

        /// <inheritdoc />
        public IFormula Visit(Negation negation)
        {
            if (Negate)
            {
                Negate = false;
                return negation.Formula.Accept(this);
            }
            else
            {
                return NegateAccept(negation.Formula);
            }
        }
    }
}