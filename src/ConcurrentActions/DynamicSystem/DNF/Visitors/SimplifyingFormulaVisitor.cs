using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Forms;

namespace DynamicSystem.DNF.Visitors
{
    internal class SimplifyingFormulaVisitor : IFormulaVisitor
    {
        public IFormula Visit(Conjunction conjunction)
        {
            return new Conjunction(conjunction.Left.Accept(this), conjunction.Right.Accept(this));
        }

        public IFormula Visit(Alternative alternative)
        {
            return new Alternative(alternative.Left.Accept(this), alternative.Right.Accept(this));
        }

        public IFormula Visit(Equivalence equivalence)
        {
            var simplifiedLeft = equivalence.Left.Accept(this);
            var simplifiedRight = equivalence.Right.Accept(this);
            return new Conjunction(
                new Alternative(new Negation(simplifiedLeft), simplifiedRight),
                new Alternative(new Negation(simplifiedRight), simplifiedLeft)
            );
        }

        public IFormula Visit(Implication implication)
        {
            return new Alternative(
                new Negation(implication.Antecedent.Accept(this)),
                implication.Consequent.Accept(this)
            );
        }

        public IFormula Visit(Constant constant)
        {
            return constant;
        }

        public IFormula Visit(Literal literal)
        {
            return literal;
        }

        public IFormula Visit(Negation negation)
        {
            return new Negation(negation.Formula.Accept(this));
        }
    }
}
