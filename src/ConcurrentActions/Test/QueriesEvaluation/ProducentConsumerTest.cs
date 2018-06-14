using System.Collections.Generic;
using System.Linq;
using DynamicSystem.QueriesEvaluation;
using Model;
using Model.Forms;
using Model.QueryLanguage;
using NUnit.Framework;

namespace Test.QueriesEvaluation
{
    [TestFixture]
    public class ProducentConsumerTest
    {
        private ICollection<Structure> _models;
        private QuerySet _queries;

        [OneTimeSetUp]
        public void SetUpProducerConsumer()
        {
            Fluent bufferEmpty = new Fluent("bufferEmpty");
            Fluent hasItem = new Fluent("hasItem");
            List<Fluent> fluents = new List<Fluent>() { bufferEmpty, hasItem };

            State s0 = new State(fluents, new List<bool>() { true, true });
            State s1 = new State(fluents, new List<bool>() { false, true });
            State s2 = new State(fluents, new List<bool>() { true, false });
            State s3 = new State(fluents, new List<bool>() { false, false });
            List<State> states = new List<State>() { s0, s1, s2, s3 };
            var initialStates = new HashSet<State>() { s0, s2 };

            Action put = new Action("Put");
            Action get = new Action("Get");
            Action consume = new Action("Consume");
            CompoundAction caPut = new CompoundAction(put);
            CompoundAction caGet = new CompoundAction(get);
            CompoundAction caConsume = new CompoundAction(consume);
            CompoundAction caPutGet = new CompoundAction(new List<Action>() { put, get });
            CompoundAction caPutConsume = new CompoundAction(new List<Action>() { put, consume });
            CompoundAction caGetConsume = new CompoundAction(new List<Action>() { get, consume });
            CompoundAction caPutGetConsume = new CompoundAction(new List<Action>() { put, get, consume });
            List<CompoundAction> compoundActions = new List<CompoundAction>()
            {
               caPut,caPutGetConsume,caConsume,caGet,caPutConsume,caGetConsume,caPutGet
            };
            var transitionFunction =
                new TransitionFunction(compoundActions, states)
                {
                    [caPut, s0] = new HashSet<State>() { s1 },
                    [caGet, s0] = new HashSet<State>() { s0 },
                    [caPutGet, s0] = new HashSet<State>() { s1 },
                    [caConsume, s0] = new HashSet<State>() { s2 },
                    [caGetConsume, s0] = new HashSet<State>() { s2 },
                    [caPutConsume, s0] = new HashSet<State>() { s3 },
                    [caPutGetConsume, s0] = new HashSet<State>() { s3 },
                    [caPut, s1] = new HashSet<State>() { s1 },
                    [caGet, s1] = new HashSet<State>() { s1 },
                    [caPutGet, s1] = new HashSet<State>() { s1 },
                    [caConsume, s1] = new HashSet<State>() { s3 },
                    [caPutConsume, s1] = new HashSet<State>() { s3 },
                    [caGetConsume, s1] = new HashSet<State>() { s3 },
                    [caPutGetConsume, s1] = new HashSet<State>() { s3 },
                    [caGet, s2] = new HashSet<State>() { s2 },
                    [caPut, s2] = new HashSet<State>() { s3 },
                    [caPutGet, s2] = new HashSet<State>() { s3 },
                    [caPut, s3] = new HashSet<State>() { s3 },
                    [caGet, s3] = new HashSet<State>() { s0 },
                    [caPutGet, s3] = new HashSet<State>() { s0 }
                };
            _models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();

            _queries = new QuerySet();
            _queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Conjunction(new Negation(new Literal(bufferEmpty)), new Negation(new Literal(hasItem))),
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            _queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Conjunction(new Negation(new Literal(bufferEmpty)), new Negation(new Literal(hasItem))),
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            _queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            _queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            _queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Alternative(new Literal(bufferEmpty), new Negation(new Literal(hasItem)))
                    ));
        }

        [Test]
        public void ProducentConsumentExistentialExecutability()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(_models, _queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentGeneralExecutability()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(_models, _queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentExistentialValue()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(_models, _queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentGeneralValue()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(_models, _queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentAccessibility()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(_models, _queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }
    }
}