using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DynamicSystem.Grammar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;
using NUnit.Framework;
using Action = Model.Action;
using Assert = NUnit.Framework.Assert;

namespace Test.Grammar
{
    [TestFixture()]
    public class GrammarTest
    {
        [Test]
        public void TestFormulaParsing()
        {
            // given
            var formulaText = "~(~((~~~a | (c & d)) -> (a <-> b)))";
            var a = new Fluent("a");
            var b = new Fluent("b");
            var c = new Fluent("c");
            var d = new Fluent("d");
            var expected = new Negation(new Negation(
                new Implication(
                    new Alternative(
                        new Negation(
                            new Negation(
                                new Negation(new Literal(a))
                            )
                        ),
                        new Conjunction(new Literal(c), new Literal(d))
                    ), 
                    new Equivalence(
                        new Literal(a), 
                        new Literal(b)
                    )
                )
            ));
            // when
            var formula = DynamicSystemParserUtils.ParseFormula(formulaText);
            // then
            Assert.AreEqual(expected, formula);
        }

        [Test]
        public void TestConstraintStatementParsing()
        {
            // given
            var input = "always ~healthy";
            var expected = new ConstraintStatement(new Negation(new Literal(new Fluent("healthy"))));
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).ConstraintStatements;
            // then
            Assert.AreEqual(expected, statements[0]);
        }

        [Test]
        public void TestEffectStatementParsing()
        {
            // given
            var input = @"
                action causes ~fluent if fluent
                impossible action if fluent
                action causes fluent
                ";
            var action = new Action("action");
            var fluent = new Fluent("fluent");
            var expected = new List<EffectStatement>()
            {
                new EffectStatement(action, new Literal(fluent), new Negation(new Literal(fluent))),
                new EffectStatement(action, Constant.Falsity, Constant.Truth),
                new EffectStatement(action, new Literal(fluent))
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).EffectStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestFluentReleaseStatementParsing()
        {
            // given
            var input = @"
                action releases fluent
                action releases fluent if fluent
            ";
            var action = new Action("action");
            var fluent = new Fluent("fluent");
            var expected = new List<FluentReleaseStatement>()
            {
                new FluentReleaseStatement(action, fluent),
                new FluentReleaseStatement(action, fluent, new Literal(fluent))
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).FluentReleaseStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestInitialValueStatementParsing()
        {
            // given
            var input = @"
                initially fluent
            ";
            var fluent = new Fluent("fluent");
            var expected = new List<InitialValueStatement>()
            {
                new InitialValueStatement(new Literal(fluent)),
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).InitialValueStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestObservationStatementParsing()
        {
            // given
            var input = @"
                observable fluent after Action
            ";
            var fluent = new Fluent("fluent");
            var action = new Action("Action");
            var expected = new List<ObservationStatement>()
            {
                new ObservationStatement(new Literal(fluent), action)
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).ObservationStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestFluentSpecificationStatement()
        {
            // given
            var input = @"
                noninertial fluent
            ";
            var fluent = new Fluent("fluent");
            var expected = new List<FluentSpecificationStatement>()
            {
                new FluentSpecificationStatement(fluent)
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).FluentSpecificationStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestValueStatementParsing()
        {
            // given
            var input = "fluent after action";
            var fluent = new Fluent("fluent");
            var action = new Action("action");
            var expected = new List<ValueStatement>()
            {
                new ValueStatement(new Literal(fluent), action)
            };
            // when
            var statements = DynamicSystemParserUtils.ParseActionDomain(input).ValueStatements;
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(statements, expected));
        }

        [Test]
        public void TestActionDomainParser()
        {
            // given
            var input = @"
                initially ~brushA
                initially ~brushB
                TakeA causes brushA & ~brushB
                TakeB causes brushB & ~brushA
                PaintA causes ~brushA if brushA
                PaintB causes ~brushB if brushB
                always ~brushA | ~brushB
            ";

            var brushA = new Fluent("brushA");
            var brushB = new Fluent("brushB");
            var takeA = new Action("TakeA");
            var takeB = new Action("TakeB");
            var paintA = new Action("PaintA");
            var paintB = new Action("PaintB");
            var initialStatements = new List<InitialValueStatement>()
            {
                new InitialValueStatement(new Negation(new Literal(brushA))),
                new InitialValueStatement(new Negation(new Literal(brushB))),
            };
            var effectStatements = new List<EffectStatement>()
            {
                new EffectStatement(takeA, new Conjunction(new Literal(brushA), new Negation(new Literal(brushB)))),
                new EffectStatement(takeB, new Conjunction(new Literal(brushB), new Negation(new Literal(brushA)))),
                new EffectStatement(paintA, new Literal(brushA), new Negation(new Literal(brushA))),
                new EffectStatement(paintB, new Literal(brushB), new Negation(new Literal(brushB))),
            };
            var constraintStatements = new List<ConstraintStatement>()
            {
                new ConstraintStatement(new Alternative(new Negation(new Literal(brushA)), new Negation(new Literal(brushB))))
            };
            // when
            var actionDomain = DynamicSystemParserUtils.ParseActionDomain(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(initialStatements, actionDomain.InitialValueStatements));
            Assert.IsTrue(Enumerable.SequenceEqual(constraintStatements, actionDomain.ConstraintStatements));
            Assert.IsTrue(Enumerable.SequenceEqual(effectStatements, actionDomain.EffectStatements));
            Assert.IsTrue(actionDomain.FluentReleaseStatements.Count +
                          actionDomain.FluentSpecificationStatements.Count + actionDomain.ObservationStatements.Count +
                          actionDomain.ValueStatements.Count == 0);
        }


        [Test]
        public void TestAccessibilityQueryParsing()
        {
            // when
            var input = "accessible ~fluent";
            var fluent = new Fluent("fluent");
            var expected = new List<AccessibilityQuery>()
            {
                new AccessibilityQuery(new Negation(new Literal(fluent)))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.AccessibilityQueries));
        }

        [Test]
        public void TestExistentialExecutabilityQueryParsing()
        {
            // when
            var input = "executable sometimes ({actionA, actionB})";
            var actionA = new Action("actionA");
            var actionB = new Action("actionB");
            var expected = new List<ExistentialExecutabilityQuery>()
            {
                new ExistentialExecutabilityQuery(new Program(new CompoundAction[]
                {
                    new CompoundAction(new Action[]{actionA, actionB}), 
                }))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.ExistentialExecutabilityQueries));
        }

        [Test]
        public void TestExistentialValueQueryParsing()
        {
            // given
            var input = "possibly ~fluent after ({actionA, actionB})";
            var fluent = new Fluent("fluent");
            var actionA = new Action("actionA");
            var actionB = new Action("actionB");
            var expected = new List<ExistentialValueQuery>()
            {
                new ExistentialValueQuery(new Negation(new Literal(fluent)), new Program(new List<CompoundAction>()
                {
                    new CompoundAction(new List<Action>()
                    {
                        actionA,
                        actionB
                    })
                }))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.ExistentialValueQueries));
        }

        [Test]
        public void TestGeneralExecutabilityQueryParsing()
        {
            // given
            var input = "executable always ({actionA, actionB})";
            var actionA = new Action("actionA");
            var actionB = new Action("actionB");
            var expected = new List<GeneralExecutabilityQuery>()
            {
                new GeneralExecutabilityQuery(new Program(new List<CompoundAction>()
                {
                    new CompoundAction(new List<Action>()
                    {
                        actionA,
                        actionB
                    })
                }))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.GeneralExecutabilityQueries));
        }

        [Test]
        public void TestGeneralValueQueryParsing()
        {
            // given
            var input = "necessary fluent after ({actionA, actionB})";
            var fluent = new Fluent("fluent");
            var literal = new Literal(fluent);
            var actionA = new Action("actionA");
            var actionB = new Action("actionB");
            var expected = new List<GeneralValueQuery>()
            {
                new GeneralValueQuery(literal, new Program(new List<CompoundAction>()
                {
                    new CompoundAction(new List<Action>()
                    {
                        actionA,
                        actionB
                    })
                }))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.GeneralValueQueries));
        }

        [Test]
        public void TestQuerySetParser()
        {
            var input = @"
                possibly brushA after ({TakeA, TakeB})
                possibly brushA after ()
                possibly brushA after ({})
                possibly brushA after ({TakeA, TakeB},{TakeA, TakeB, Paint})
            ";
            var fluent = new Fluent("brushA");
            var literal = new Literal(fluent);
            var takeA = new Action("TakeA");
            var takeB = new Action("TakeB");
            var paint = new Action("Paint");
            var expected = new List<ExistentialValueQuery>()
            {
                new ExistentialValueQuery(literal,
                    new Program(new CompoundAction[] {new CompoundAction(new Action[] {takeA, takeB}),})),
                new ExistentialValueQuery(literal, new Program(new CompoundAction[] { })),
                new ExistentialValueQuery(literal,
                    new Program(new CompoundAction[] {new CompoundAction(new Action[] { })})),
                new ExistentialValueQuery(literal, new Program(new CompoundAction[]
                {
                    new CompoundAction(new Action[] {takeA, takeB}),
                    new CompoundAction(new Action[] {takeA, takeB, paint}),
                }))
            };
            // when
            var querySet = DynamicSystemParserUtils.ParseQuerySet(input);
            // then
            Assert.IsTrue(Enumerable.SequenceEqual(expected, querySet.ExistentialValueQueries));
        }
    }
}