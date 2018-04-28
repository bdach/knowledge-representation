using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;
using Action = System.Action;

namespace DynamicSystem.DNF.Visitors
{
    /// <summary>
    /// Implementation of <see cref="IFormulaVisitor"/> that generates all <see cref="NaryConjunction"/> in a <see cref="IFormula"/>.
    /// Can only be used on formulae in disjunctive normal form.
    /// </summary>
    internal class NaryConjunctionGeneratingFormulaVisitor : IFormulaVisitor
    {
        private readonly Action<NaryConjunction> _conjunctionAcceptor;

        /// <summary>
        /// Creates new instance of <see cref="NaryConjunctionGeneratingFormulaVisitor"/>
        /// </summary>
        /// <param name="conjunctionAcceptor">Generation action for instances of <see cref="NaryConjunction"/></param>
        public NaryConjunctionGeneratingFormulaVisitor(Action<NaryConjunction> conjunctionAcceptor)
        {
            _conjunctionAcceptor = conjunctionAcceptor;
        }

        /// <inheritdoc />
        public IFormula Visit(Conjunction conjunction)
        {
            var literals = new List<Literal>();
            var constants = new List<Constant>();
            conjunction.Accept(new LeafGeneratingFormulaVisitor(literals.Add, constants.Add));
            _conjunctionAcceptor(new NaryConjunction(literals, constants));
            return conjunction;
        }

        /// <inheritdoc />
        public IFormula Visit(Alternative alternative)
        {
            alternative.Left.Accept(this);
            alternative.Right.Accept(this);
            return alternative;
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
            _conjunctionAcceptor(new NaryConjunction(new List<Literal>() { }, new List<Constant>() {constant}));
            return constant;
        }

        /// <inheritdoc />
        public IFormula Visit(Literal literal)
        {
            _conjunctionAcceptor(new NaryConjunction(new List<Literal>() {literal}, new List<Constant>()));
            return literal;
        }

        /// <inheritdoc />
        public IFormula Visit(Negation negation)
        {
            throw new InvalidOperationException("negation not expected");
        }

        /// <summary>
        /// Helper implementation of <see cref="IFormulaVisitor"/> that generates all <see cref="Literal"/> and <see cref="Constant"/>
        /// from a n-ary conjunction represented as a <see cref="IFormula"/>.
        /// </summary>
        private class LeafGeneratingFormulaVisitor : IFormulaVisitor
        {
            private readonly Action<Literal> _literalAcceptor;
            private readonly Action<Constant> _constantAcceptor;

            /// <summary>
            /// Creates new instance of <see cref="LeafGeneratingFormulaVisitor"/>
            /// </summary>
            /// <param name="literalAcceptor">Generation action of <see cref="Literal"/></param>
            /// <param name="constantAcceptor">Generation action of <see cref="Constant"/></param>
            public LeafGeneratingFormulaVisitor(Action<Literal> literalAcceptor, Action<Constant> constantAcceptor)
            {
                _literalAcceptor = literalAcceptor;
                _constantAcceptor = constantAcceptor;
            }

            /// <inheritdoc />
            public IFormula Visit(Conjunction conjunction)
            {
                conjunction.Left.Accept(this);
                conjunction.Right.Accept(this);
                return conjunction;
            }

            /// <inheritdoc />
            public IFormula Visit(Alternative alternative)
            {
                throw new InvalidOperationException("alternative not expected");
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
                _constantAcceptor(constant);
                return constant;
            }

            /// <inheritdoc />
            public IFormula Visit(Literal literal)
            {
                _literalAcceptor(literal);
                return literal;
            }

            /// <inheritdoc />
            public IFormula Visit(Negation negation)
            {
                throw new InvalidOperationException("negation not expected");
            }
        }
    }
}