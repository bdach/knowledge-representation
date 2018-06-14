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
    public class OpeningDoorTest
    {
        private QueryResolution _resolution;

        /// <summary>
        /// Creates and evaluates scenario for conflicting actions.
        /// </summary>
        /// <remarks>
        /// New objects are created everywhere to avoid unwanted reference interference (and possibly catch errors related to that).
        /// </remarks>
        [OneTimeSetUp]
        public void SetUpOpeningDoor()
        {
            // define fluents
            var hasKey = new Fluent("hasKey");
            var doorOpen = new Fluent("doorOpen");
            var fluents = new List<Fluent> { hasKey, doorOpen };

            // define actions
            var open = new Action("OPEN");
            var close = new Action("CLOSE");
            var actions = new List<Action> { open, close };

            // create language signature
            var signature = new Signature(fluents, actions);

            // define action domain
            var actionDomain = new ActionDomain();
            // initially hasKey
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(hasKey)));
            // OPEN causes doorOpen if hasKey
            actionDomain.EffectStatements.Add(new EffectStatement(open, new Literal(hasKey), new Literal(doorOpen)));
            // CLOSE causes ~doorOpen if hasKey
            actionDomain.EffectStatements.Add(new EffectStatement(close, new Literal(hasKey), new Negation(new Literal(doorOpen))));

            // define query set
            var queries = new QuerySet();
            // possibly doorOpen after ({OPEN, CLOSE})
            queries.ExistentialValueQueries.Add(new ExistentialValueQuery(new Literal(doorOpen), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // possibly ~doorOpen after ({OPEN, CLOSE})
            queries.ExistentialValueQueries.Add(new ExistentialValueQuery(new Negation(new Literal(doorOpen)), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // necessary doorOpen after ({OPEN, CLOSE})
            queries.GeneralValueQueries.Add(new GeneralValueQuery(new Literal(doorOpen), new Program(new List<CompoundAction> { new CompoundAction(actions) })));
            // necessary ~doorOpen after ({OPEN, CLOSE})
            queries.GeneralValueQueries.Add(new GeneralValueQuery(new Negation(new Literal(doorOpen)), new Program(new List<CompoundAction> { new CompoundAction(actions) })));

            // evaluate queries
            _resolution = DynamicSystem.QueryResolver.ResolveQueries(signature, actionDomain, queries);
        }

        [Test]
        public void OpeningDoorExistentialValueQueryOpen()
        {
            Assert.AreEqual(true, _resolution.ExistentialValueQueryResults[0].Item2);
        }

        [Test]
        public void OpeningDoorExistentialValueQueryClosed()
        {
            Assert.AreEqual(true, _resolution.ExistentialValueQueryResults[1].Item2);
        }

        [Test]
        public void OpeningDoorGeneralValueQueryOpen()
        {
            Assert.AreEqual(false, _resolution.GeneralValueQueryResults[0].Item2);
        }

        [Test]
        public void OpeningDoorGeneralValueQueryClosed()
        {
            Assert.AreEqual(false, _resolution.GeneralValueQueryResults[1].Item2);
        }
    }
}