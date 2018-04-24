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
    internal class NaryConjunctionGeneratoringFormulaVisitor : IFormulaVisitor
    {
        private readonly Action<NaryConjunction> _conjunctionAcceptor;

        public NaryConjunctionGeneratoringFormulaVisitor(Action<NaryConjunction> conjunctionAcceptor)
        {
            _conjunctionAcceptor = conjunctionAcceptor;
        }

        public IFormula Visit(Conjunction conjunction)
        {
            var literals = new List<Literal>();
            var constants = new List<Constant>();
            conjunction.Accept(new LeafGeneratingFormulaVisitor(literals.Add, constants.Add));
            _conjunctionAcceptor(new NaryConjunction(literals, constants));
            return conjunction;
        }

        public IFormula Visit(Alternative alternative)
        {
            alternative.Left.Accept(this);
            alternative.Right.Accept(this);
            return alternative;
        }

        public IFormula Visit(Equivalence equivalence)
        {
            throw new InvalidOperationException("equivalence not expected");
        }

        public IFormula Visit(Implication implication)
        {
            throw new InvalidOperationException("implication not expected");
        }

        public IFormula Visit(Constant constant)
        {
            throw new InvalidOperationException("constant not expected");
        }

        public IFormula Visit(Literal literal)
        {
            throw new InvalidOperationException("literal not expected");
        }

        public IFormula Visit(Negation negation)
        {
            throw new InvalidOperationException("negation not expected");
        }


        private class LeafGeneratingFormulaVisitor : IFormulaVisitor
        {
            private readonly Action<Literal> _literalAcceptor;
            private readonly Action<Constant> _constantAcceptor;

            public LeafGeneratingFormulaVisitor(Action<Literal> literalAcceptor, Action<Constant> constantAcceptor)
            {
                _literalAcceptor = literalAcceptor;
                _constantAcceptor = constantAcceptor;
            }

            public IFormula Visit(Conjunction conjunction)
            {
                conjunction.Left.Accept(this);
                conjunction.Right.Accept(this);
                return conjunction;
            }

            public IFormula Visit(Alternative alternative)
            {
                throw new InvalidOperationException("alternative not expected");
            }

            public IFormula Visit(Equivalence equivalence)
            {
                throw new InvalidOperationException("equivalence not expected");
            }

            public IFormula Visit(Implication implication)
            {
                throw new InvalidOperationException("implication not expected");
            }

            public IFormula Visit(Constant constant)
            {
                _constantAcceptor(constant);
                return constant;
            }

            public IFormula Visit(Literal literal)
            {
                _literalAcceptor(literal);
                return literal;
            }

            public IFormula Visit(Negation negation)
            {
                throw new InvalidOperationException("negation not expected");
            }
        }
    }
}
