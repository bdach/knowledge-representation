using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Action = Model.Action;

namespace DynamicSystem.Grammar
{
    /// <summary>
    /// Implementation of DynamicSystem AST visitor which parses <see cref="ActionDomain"/>
    /// </summary>
    internal class ActionDomainParsingVisitor : DynamicSystemBaseVisitor<Object>
    {
        public override object VisitInitialValueStatement(DynamicSystemParser.InitialValueStatementContext context)
        {
            IFormula formula = context.formula().Accept(new FormulaParsingVisitor());
            return new InitialValueStatement(formula);
        }

        public override object VisitValueStatement(DynamicSystemParser.ValueStatementContext context)
        {
            var action = new Action(context.action().GetText());
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new ValueStatement(formula, action);
        }

        public override object VisitObservationStatement(DynamicSystemParser.ObservationStatementContext context)
        {
            var action = new Action(context.action().GetText());
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new ObservationStatement(formula, action);
        }

        public override object VisitEffectStatement(DynamicSystemParser.EffectStatementContext context)
        {
            var action = new Action(context.action().GetText());
            var condition = context.condition() != null
                ? context.condition().formula().Accept(new FormulaParsingVisitor())
                : null;
            var formula = context.formula() != null ? context.formula().Accept(new FormulaParsingVisitor()) : null;
            var postCondition = formula ?? Constant.Falsity;
            return condition != null
                ? new EffectStatement(action, condition, postCondition)
                : new EffectStatement(action, postCondition);
        }

        public override object VisitFluentReleaseStatement(
            DynamicSystemParser.FluentReleaseStatementContext context)
        {
            var action = new Action(context.action().GetText());
            var fluent = new Fluent(context.fluent().GetText());
            var condition = context.condition() != null
                ? context.condition().formula().Accept(new FormulaParsingVisitor())
                : null;
            return condition == null
                ? new FluentReleaseStatement(action, fluent)
                : new FluentReleaseStatement(action, fluent, condition);
        }

        public override object VisitConstraintStatement(DynamicSystemParser.ConstraintStatementContext context)
        {
            var formula = context.formula().Accept(new FormulaParsingVisitor());
            return new ConstraintStatement(formula);
        }

        public override object VisitFluentSpecificationStatement(
            DynamicSystemParser.FluentSpecificationStatementContext context)
        {
            var fluent = new Fluent(context.fluent().GetText());
            return new FluentSpecificationStatement(fluent);
        }

        public override object VisitActionDomain(DynamicSystemParser.ActionDomainContext context)
        {
            var actionDomain = new ActionDomain();

            foreach (var stmt in context.statement())
            {
                var statement = stmt.Accept(this);
                if (statement is ConstraintStatement constraintStatement)
                {
                    actionDomain.ConstraintStatements.Add(constraintStatement);
                }
                else if (statement is EffectStatement effectStatement)
                {
                    actionDomain.EffectStatements.Add(effectStatement);
                }
                else if (statement is FluentReleaseStatement releaseStatement)
                {
                    actionDomain.FluentReleaseStatements.Add(releaseStatement);
                }
                else if (statement is FluentSpecificationStatement specificationStatement)
                {
                    actionDomain.FluentSpecificationStatements.Add(specificationStatement);
                }
                else if (statement is InitialValueStatement valueStatement)
                {
                    actionDomain.InitialValueStatements.Add(valueStatement);
                }
                else if (statement is ValueStatement statement1)
                {
                    actionDomain.ValueStatements.Add(statement1);
                }
                else if (statement is ObservationStatement observationStatement)
                {
                    actionDomain.ObservationStatements.Add(observationStatement);
                }
            }

            return actionDomain;
        }

        public override object VisitStatement(DynamicSystemParser.StatementContext context)
        {
            if (context.constraintStatement() != null)
            {
                return context.constraintStatement().Accept(this);
            }
            else if (context.effectStatement() != null)
            {
                return context.effectStatement().Accept(this);
            }
            else if (context.fluentReleaseStatement() != null)
            {
                return context.fluentReleaseStatement().Accept(this);
            }
            else if (context.fluentSpecificationStatement() != null)
            {
                return context.fluentSpecificationStatement().Accept(this);
            }
            else if (context.initialValueStatement() != null)
            {
                return context.initialValueStatement().Accept(this);
            }
            else if (context.valueStatement() != null)
            {
                return context.valueStatement().Accept(this);
            }
            else if (context.observationStatement() != null)
            {
                return context.observationStatement().Accept(this);
            }

            throw new Exception("invalid empty statement");
        }
    }
}