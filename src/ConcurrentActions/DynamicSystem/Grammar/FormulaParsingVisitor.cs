using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.Grammar;
using Model;
using Model.Forms;

namespace DynamicSystem.Grammar
{
    /// <summary>
    /// Implementation of DynamicSystem AST visitor which parses <see cref="IFormula"/>
    /// </summary>
    internal class FormulaParsingVisitor : DynamicSystemBaseVisitor<IFormula>
    {
        public override IFormula VisitConstant(DynamicSystemParser.ConstantContext context)
        {
            return context.TRUTH() != null ? Constant.Truth : Constant.Falsity;
        }

        public override IFormula VisitLiteral(DynamicSystemParser.LiteralContext context)
        {
            var fluentContext = context.fluent();
            return new Literal(new Fluent(fluentContext.Start.Text), context.NEGATION() != null);
        }

        public override IFormula VisitFormula(DynamicSystemParser.FormulaContext context)
        {
            if (context.formula() != null)
            {
                return new Equivalence(
                    context.formula().Accept(this),
                    context.implication().Accept(this)
                );
            }
            else
            {
                return context.implication().Accept(this);
            }
        }

        public override IFormula VisitImplication(DynamicSystemParser.ImplicationContext context)
        {
            if (context.implication() != null)
            {
                return new Implication(context.implication().Accept(this), context.alternative().Accept(this));
            }
            else
            {
                return context.alternative().Accept(this);
            }
        }

        public override IFormula VisitAlternative(DynamicSystemParser.AlternativeContext context)
        {
            if (context.alternative() != null)
            {
                return new Alternative(context.alternative().Accept(this), context.conjunction().Accept(this));
            }
            else
            {
                return context.conjunction().Accept(this);
            }
        }

        public override IFormula VisitConjunction(DynamicSystemParser.ConjunctionContext context)
        {
            if (context.conjunction() != null)
            {
                return new Conjunction(context.conjunction().Accept(this), context.negation().Accept(this));
            }
            else
            {
                return context.negation().Accept(this);
            }
        }

        public override IFormula VisitNegation(DynamicSystemParser.NegationContext context)
        {
            if (context.formula() != null)
            {
                var formula = context.formula().Accept(this);
                if (context.NEGATION() != null)
                {
                    formula = new Negation(formula);
                }

                return formula;
            }
            else if (context.literal() != null)
            {
                return context.literal().Accept(this);
            }
            else
            {
                return context.constant().Accept(this);
            }
        }
    }
}