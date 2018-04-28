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
        private IFormulaVisitor ChildVisitor { get; }

        /// <summary>
        /// Creates new instance of <see cref="NegationPropagatingFormulaVisitor"/>
        /// </summary>
        public NegationPropagatingFormulaVisitor()
        {
            ChildVisitor = new NegatingFormulaVisitor(this);
        }

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
            return new Equivalence(equivalence.Left.Accept(this), equivalence.Right.Accept(this));
        }

        /// <inheritdoc />
        public IFormula Visit(Implication implication)
        {
            return new Implication(implication.Antecedent.Accept(this), implication.Consequent.Accept(this));
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
            return negation.Formula.Accept(ChildVisitor);
        }

        /// <summary>
        /// Private implementation of <see cref="IFormulaVisitor"/> that negates visited formula
        /// </summary>
        private class NegatingFormulaVisitor : IFormulaVisitor
        {
            private IFormulaVisitor ParentVisitor { get; }

            /// <summary>
            /// Creates new instance of <see cref="NegatingFormulaVisitor"/>
            /// </summary>
            /// <param name="parentVisitor"></param>
            public NegatingFormulaVisitor(IFormulaVisitor parentVisitor)
            {
                ParentVisitor = parentVisitor;
            }

            /// <inheritdoc />
            public IFormula Visit(Conjunction conjunction)
            {
                return new Alternative(conjunction.Left.Accept(this), conjunction.Right.Accept(this));
            }

            /// <inheritdoc />
            public IFormula Visit(Alternative alternative)
            {
                return new Conjunction(alternative.Left.Accept(this), alternative.Right.Accept(this));
            }

            /// <inheritdoc />
            public IFormula Visit(Equivalence equivalence)
            {
                return new Alternative(
                    new Implication(equivalence.Left, equivalence.Right).Accept(this),
                    new Implication(equivalence.Right, equivalence.Left).Accept(this)
                );
            }

            /// <inheritdoc />
            public IFormula Visit(Implication implication)
            {
                return new Conjunction(implication.Antecedent.Accept(ParentVisitor),
                    implication.Consequent.Accept(this));
            }

            /// <inheritdoc />
            public IFormula Visit(Constant constant)
            {
                return constant.Equals(Constant.Falsity) ? Constant.Truth : Constant.Falsity;
            }

            /// <inheritdoc />
            public IFormula Visit(Literal literal)
            {
                return new Literal(literal.Fluent, !literal.Negated);
            }

            /// <inheritdoc />
            public IFormula Visit(Negation negation)
            {
                return negation.Formula.Accept(ParentVisitor);
            }
        }
    }
}