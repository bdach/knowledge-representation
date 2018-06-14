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
    public class TwoPaintersTest
    {
        private ICollection<Structure> _models;
        private QuerySet _queries;

        [OneTimeSetUp]
        public void SetUpTwoPainters()
        {
            Fluent brushA = new Fluent("brushA");
            Fluent brushB = new Fluent("brushB");
            List<Fluent> fluents = new List<Fluent>() { brushA, brushB };

            State s0 = new State(fluents, new List<bool>() { false, false });
            State s1 = new State(fluents, new List<bool>() { true, false });
            State s2 = new State(fluents, new List<bool>() { false, true });
            List<State> states = new List<State>() { s0, s1, s2 };
            var initialStates = new HashSet<State>() { s0 };

            Action takeA = new Action("TakeA");
            Action takeB = new Action("TakeB");
            Action paint = new Action("Paint");
            CompoundAction caTakeA = new CompoundAction(takeA);
            CompoundAction caTakeB = new CompoundAction(takeB);
            CompoundAction caPaint = new CompoundAction(paint);
            CompoundAction caTakeATakeB = new CompoundAction(new List<Action>() { takeA, takeB });
            CompoundAction caTakeAPaint = new CompoundAction(new List<Action>() { takeA, paint });
            CompoundAction caTakeBPaint = new CompoundAction(new List<Action>() { takeB, paint });
            CompoundAction caTakeATakeBPaint = new CompoundAction(new List<Action>() { takeA, takeB, paint });
            List<CompoundAction> compoundActions = new List<CompoundAction>()
            {
                caTakeA, caTakeB, caPaint, caTakeAPaint, caTakeBPaint, caTakeATakeB,  caTakeATakeBPaint
            };
            var transitionFunction =
                new TransitionFunction(compoundActions, states)
                {
                    [caPaint, s0] = new HashSet<State>() { s0 },
                    [caTakeA, s0] = new HashSet<State>() { s1 },
                    [caTakeB, s0] = new HashSet<State>() { s2 },
                    [caTakeATakeB, s0] = new HashSet<State>() { s1, s2 },
                    [caTakeAPaint, s0] = new HashSet<State>() { s1 },
                    [caTakeBPaint, s0] = new HashSet<State>() { s2 },
                    [caTakeATakeBPaint, s0] = new HashSet<State>() { s1, s2 },
                    [caPaint, s1] = new HashSet<State>() { s0 },
                    [caTakeA, s1] = new HashSet<State>() { s1 },
                    [caTakeB, s1] = new HashSet<State>() { s2 },
                    [caTakeATakeB, s1] = new HashSet<State>() { s1 },
                    [caTakeAPaint, s1] = new HashSet<State>() { s1 },
                    [caTakeBPaint, s1] = new HashSet<State>() { s2 },
                    [caTakeATakeBPaint, s1] = new HashSet<State>() { s1 },
                    [caPaint, s2] = new HashSet<State>() { s0 },
                    [caTakeA, s2] = new HashSet<State>() { s1 },
                    [caTakeB, s2] = new HashSet<State>() { s2 },
                    [caTakeATakeB, s2] = new HashSet<State>() { s2 },
                    [caTakeAPaint, s2] = new HashSet<State>() { s1 },
                    [caTakeBPaint, s2] = new HashSet<State>() { s2 },
                    [caTakeATakeBPaint, s2] = new HashSet<State>() { s2 },
                };
            _models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();

            _queries = new QuerySet();
            _queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Literal(brushA),
                    new Program(new List<CompoundAction>() { caTakeATakeB })
                    ));
            _queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Conjunction(new Negation(new Literal(brushA)), new Negation(new Literal(brushB))),
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeBPaint })
                    ));
            _queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeB, caTakeA })
                    ));
            _queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeB, caTakeBPaint })
                    ));
            _queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Conjunction(new Literal(brushA), new Literal(brushB))
                    ));
        }

        [Test]
        public void TwoPaintersExistentialExecutability()
        {
            //given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(_models, _queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersGeneralExecutability()
        {
            //given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(_models, _queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersExistentialValue()
        {
            //given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(_models, _queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersGeneralValue()
        {
            //given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(_models, _queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void TwoPaintersAccessibility()
        {
            //given
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(_models, _queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }
    }
}