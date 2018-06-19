using System.Collections.Generic;
using DynamicSystem;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using Model.QueryLanguage;
using NUnit.Framework;

namespace Test.QueriesEvaluation
{
    [TestFixture]
    public class AlternativesTest
    {
        private QueryResolution _resolution;

        /// <summary>
        /// Creates and evaluates scenario for conflicting actions.
        /// </summary>
        /// <remarks>
        /// New objects are created everywhere to avoid unwanted reference interference (and possibly catch errors related to that).
        /// </remarks>
        [OneTimeSetUp]
        public void SetUpAlternatives()
        {
            // define fluents
            var f = new Fluent("f");
            var g = new Fluent("g");
            var h = new Fluent("h");
            var fluents = new List<Fluent> { f, g, h };

            // define actions
            var a = new Action("A");
            var b = new Action("B");
            var actions = new List<Action> { a, b };

            // create language signature
            var signature = new Signature(fluents, actions);

            // define action domain
            var actionDomain = new ActionDomain();
            // initially f
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(f)));
            // A causes g|h if f
            actionDomain.EffectStatements.Add(new EffectStatement(a, new Literal(f), new Alternative(new Literal(g), new Literal(h))));
            // B causes g|h if f
            actionDomain.EffectStatements.Add(new EffectStatement(b, new Literal(f), new Alternative(new Literal(g), new Literal(h))));
            // noninertial g
            actionDomain.FluentSpecificationStatements.Add(new FluentSpecificationStatement(g));
            // noninertial h
            actionDomain.FluentSpecificationStatements.Add(new FluentSpecificationStatement(h));

            // define query set
            var queries = new QuerySet();
            // possibly g after ({A, B})
            queries.ExistentialValueQueries.Add(new ExistentialValueQuery(new Literal(g), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // possibly h after ({A, B})
            queries.ExistentialValueQueries.Add(new ExistentialValueQuery(new Literal(h), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // necessary g after ({A, B})
            queries.GeneralValueQueries.Add(new GeneralValueQuery(new Literal(g), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // necessary h after ({A, B})
            queries.GeneralValueQueries.Add(new GeneralValueQuery(new Literal(h), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // necessary g|h after ({A, B})
            queries.GeneralValueQueries.Add(new GeneralValueQuery(new Alternative(new Literal(g), new Literal(h)), new Program(new List<CompoundAction> { new CompoundAction(actions) })));

            // evaluate queries
            _resolution = DynamicSystem.QueryResolver.ResolveQueries(signature, actionDomain, queries);
        }

        [Test]
        public void AlternativesExistentialValueQueryG()
        {
            Assert.AreEqual(true, _resolution.ExistentialValueQueryResults[0].Item2);
        }

        [Test]
        public void AlternativesExistentialValueQueryH()
        {
            Assert.AreEqual(true, _resolution.ExistentialValueQueryResults[1].Item2);
        }

        [Test]
        public void AlternativesGeneralValueQueryG()
        {
            Assert.AreEqual(false, _resolution.GeneralValueQueryResults[0].Item2);
        }

        [Test]
        public void AlternativesGeneralValueQueryH()
        {
            Assert.AreEqual(false, _resolution.GeneralValueQueryResults[1].Item2);
        }

        [Test]
        public void AlternativesGeneralValueQueryGorH()
        {
            Assert.AreEqual(true, _resolution.GeneralValueQueryResults[2].Item2);
        }
    }
}
