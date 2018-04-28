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
    /// Implementation of <see cref="IFormulaVisitor"/> that converts <see cref="Implication"/>
    /// and <see cref="Equivalence"/> into equivalent formulae that uses <see cref="Conjunction"/> and <see cref="Alternative"/>
    /// </summary>
    internal class SimplifyingFormulaVisitor : IFormulaVisitor
    {
        /// <inheritdoc />
        public IFormula Visit(Conjunction conjunction)
        {
            return new Conjunction(conjunction.Left.Accept(this), conjunction.Right.Accept(this));
        }

        /// <inheritdoc />
        public IFormula Visit(Alternative alternative)
        {
            return new Alternative(alternative.Left.Accept(this), alternative.Right.Accept(this));
        }

        /// <inheritdoc />
        public IFormula Visit(Equivalence equivalence)
        {
            var simplifiedLeft = equivalence.Left.Accept(this);
            var simplifiedRight = equivalence.Right.Accept(this);
            return new Conjunction(
                new Alternative(new Negation(simplifiedLeft), simplifiedRight),
                new Alternative(new Negation(simplifiedRight), simplifiedLeft)
            );
        }

        /// <inheritdoc />
        public IFormula Visit(Implication implication)
        {
            return new Alternative(
                new Negation(implication.Antecedent.Accept(this)),
                implication.Consequent.Accept(this)
            );
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
            return new Negation(negation.Formula.Accept(this));
        }
    }
}