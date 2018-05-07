using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.QueryLanguage;
using Action = Model.Action;

namespace DynamicSystem.Grammar
{
    /// Implementation of DynamicSystem AST visitor which parses <see cref="QuerySet"/>
    internal class QuerySetParsingVisitor : DynamicSystemBaseVisitor<Object>
    {
        public override object VisitQuerySet(DynamicSystemParser.QuerySetContext context)
        {
            var querySet = new QuerySet();

            foreach (var query in context.query())
            {
                var q = query.Accept(this);
                if (q is AccessibilityQuery accessibilityQuery)
                {
                    querySet.AccessibilityQueries.Add(accessibilityQuery);
                }
                else if (q is GeneralValueQuery generalValueQuery)
                {
                    querySet.GeneralValueQueries.Add(generalValueQuery);
                }
                else if (q is ExistentialValueQuery existentialValueQuery)
                {
                    querySet.ExistentialValueQueries.Add(existentialValueQuery);
                }
                else if (q is GeneralExecutabilityQuery generalExecutabilityQuery)
                {
                    querySet.GeneralExecutabilityQueries.Add(generalExecutabilityQuery);
                }
                else if (q is ExistentialExecutabilityQuery existentialExecutabilityQuery)
                {
                    querySet.ExistentialExecutabilityQueries.Add(existentialExecutabilityQuery);
                }
            }

            return querySet;
        }

        public override object VisitQuery(DynamicSystemParser.QueryContext context)
        {
            if (context.existentialExecutabilityQuery() != null)
            {
                return context.existentialExecutabilityQuery().Accept(this);
            }
            else if (context.existentialValueQuery() != null)
            {
                return context.existentialValueQuery().Accept(this);
            }
            else if (context.generalExecutabilityQuery() != null)
            {
                return context.generalExecutabilityQuery().Accept(this);
            }
            else if (context.generalValueQuery() != null)
            {
                return context.generalValueQuery().Accept(this);
            }
            else if (context.accessibilityQuery() != null)
            {
                return context.accessibilityQuery().Accept(this);
            }

            throw new Exception("invalid query");
        }

        public override object VisitAccessibilityQuery(DynamicSystemParser.AccessibilityQueryContext context)
        {
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new AccessibilityQuery(formula);
        }

        public override object VisitGeneralExecutabilityQuery(
            DynamicSystemParser.GeneralExecutabilityQueryContext context)
        {
            var program = new Program(context.compoundActions().Accept(this) as List<CompoundAction>);
            return new GeneralExecutabilityQuery(program);
        }

        public override object VisitExistentialExecutabilityQuery(
            DynamicSystemParser.ExistentialExecutabilityQueryContext context)
        {
            var program = new Program(context.compoundActions().Accept(this) as List<CompoundAction>);
            return new ExistentialExecutabilityQuery(program);
        }

        public override object VisitGeneralValueQuery(DynamicSystemParser.GeneralValueQueryContext context)
        {
            var program = new Program(context.compoundActions().Accept(this) as List<CompoundAction>);
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new GeneralValueQuery(formula, program);
        }

        public override object VisitExistentialValueQuery(DynamicSystemParser.ExistentialValueQueryContext context)
        {
            var program = new Program(context.compoundActions().Accept(this) as List<CompoundAction>);
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new ExistentialValueQuery(formula, program);
        }

        public override object VisitCompoundAction(DynamicSystemParser.CompoundActionContext context)
        {
            return new CompoundAction(context.actions().Accept(this) as List<Action>);
        }

        public override object VisitActions(DynamicSystemParser.ActionsContext context)
        {
            return context.action().ToList().Select(a => new Model.Action(a.GetText())).ToList();
        }

        public override object VisitCompoundActions(DynamicSystemParser.CompoundActionsContext context)
        {
            return context.compoundAction().ToList().Select(a => a.Accept(this) as CompoundAction).ToList();
        }
    }
}