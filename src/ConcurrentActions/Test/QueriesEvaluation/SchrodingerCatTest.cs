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
    public class SchrodingerCatTest
    {
        private ICollection<Structure> _models;
        private QuerySet _queries;

        [OneTimeSetUp]
        public void SetUpSchrodingerCat()
        {
            Fluent alive = new Fluent("alive");
            Fluent purring = new Fluent("purring");
            List<Fluent> fluents = new List<Fluent>() { alive, purring };

            State s0 = new State(fluents, new List<bool>() { true, true });
            State s1 = new State(fluents, new List<bool>() { true, false });
            State s2 = new State(fluents, new List<bool>() { false, false });
            List<State> states = new List<State>() { s0, s1, s2 };
            var initialStates = new HashSet<State>() { s0, s1 };

            Action peek = new Action("Peek");
            Action pet = new Action("Pet");
            CompoundAction caPeek = new CompoundAction(peek);
            CompoundAction caPet = new CompoundAction(pet);
            CompoundAction caPeekPet = new CompoundAction(new List<Action>() { peek, pet });
            List<CompoundAction> compoundActions = new List<CompoundAction>()
            {
                caPeek,caPet,caPeekPet
            };
            var transitionFunction =
                new TransitionFunction(compoundActions, states)
                {
                    [caPeek, s0] = new HashSet<State>() { s0, s1, s2 },
                    [caPet, s0] = new HashSet<State>() { s0 },
                    [caPeekPet, s0] = new HashSet<State>() { s0, s1, s2 },
                    [caPeek, s1] = new HashSet<State>() { s1, s2 },
                    [caPet, s1] = new HashSet<State>() { s0 },
                    [caPeekPet, s1] = new HashSet<State>() { s1, s2 },
                    [caPeek, s2] = new HashSet<State>() { s2 },
                    [caPet, s2] = new HashSet<State>() { s2 },
                    [caPeekPet, s2] = new HashSet<State>() { s2 },
                };
            _models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();

            _queries = new QuerySet();
            _queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Negation(new Literal(alive)),
                    new Program(new List<CompoundAction>() { caPet, caPeek })
                    ));
            _queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Literal(purring),
                    new Program(new List<CompoundAction>() { caPeekPet })
                    ));
            _queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPeek, caPet, caPeekPet })
                    ));
            _queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPeek, caPet, caPeekPet })
                    ));
            _queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Literal(purring)
                    ));
        }

        [Test]
        public void SchrodingerCatExistentialExecutability()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(_models, _queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatGeneralExecutability()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(_models, _queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatExistentialValue()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(_models, _queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatGeneralValue()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(_models, _queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void SchrodingerCatAccessibility()
        {
            // given
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(_models, _queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }
    }
}