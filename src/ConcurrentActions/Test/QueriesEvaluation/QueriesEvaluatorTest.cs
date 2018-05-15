using System.Collections.Generic;
using System.Linq;
using DynamicSystem.QueriesEvaluation;
using Model;
using Model.Forms;
using Model.QueryLanguage;
using NUnit.Framework;
using Action = Model.Action;

namespace Test.QueriesEvaluation
{
    [TestFixture]
    class QueriesEvaluatorTest
    {
        public void BuildProducerConsumer(out ICollection<Structure> models, out QuerySet queries)
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
                    [caPut, s0] = new HashSet<State>() {s1},
                    [caGet, s0] = new HashSet<State>() {s0},
                    [caPutGet, s0] = new HashSet<State>() {s1},
                    [caConsume, s0] = new HashSet<State>() {s2},
                    [caGetConsume, s0] = new HashSet<State>() {s2},
                    [caPutConsume, s0] = new HashSet<State>() {s3},
                    [caPutGetConsume, s0] = new HashSet<State>() {s3},
                    [caPut, s1] = new HashSet<State>() {s1},
                    [caGet, s1] = new HashSet<State>() {s1},
                    [caPutGet, s1] = new HashSet<State>() {s1},
                    [caConsume, s1] = new HashSet<State>() {s3},
                    [caPutConsume, s1] = new HashSet<State>() {s3},
                    [caGetConsume, s1] = new HashSet<State>() {s3},
                    [caPutGetConsume, s1] = new HashSet<State>() {s3},
                    [caGet, s2] = new HashSet<State>() {s2},
                    [caPut, s2] = new HashSet<State>() {s3},
                    [caPutGet, s2] = new HashSet<State>() {s3},
                    [caPut, s3] = new HashSet<State>() {s3},
                    [caGet, s3] = new HashSet<State>() {s0},
                    [caPutGet, s3] = new HashSet<State>() {s0}
                };
            models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();

            queries = new QuerySet();
            queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Conjunction(new Negation(new Literal(bufferEmpty)), new Negation(new Literal(hasItem))),
                    new Program(new List<CompoundAction> () {caPut, caGetConsume})
                    ));
            queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Conjunction(new Negation(new Literal(bufferEmpty)), new Negation(new Literal(hasItem))),
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPut, caGetConsume })
                    ));
            queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Alternative(new Literal(bufferEmpty), new Negation(new Literal(hasItem)) )
                    ));
        }

        [Test]
        public void ProducentConsumentExistentialExecutability()
        {
            // given
            BuildProducerConsumer(out var models, out var queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(models, queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentGeneralExecutability()
        {
            // given
            BuildProducerConsumer(out var models, out var queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(models, queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentExistentialValue()
        {
            // given
            BuildProducerConsumer(out var models, out var queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(models, queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentGeneralValue()
        {
            // given
            BuildProducerConsumer(out var models, out var queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(models, queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void ProducentConsumentAccessibility()
        {
            // given
            BuildProducerConsumer(out var models, out var queries);
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(models, queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }



        public void BuildSchrodingerCat(out ICollection<Structure> models, out QuerySet queries)
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
            CompoundAction caPeekPet = new CompoundAction(new List<Action>() {peek,pet});
            List<CompoundAction> compoundActions = new List<CompoundAction>()
            {
                caPeek,caPet,caPeekPet
            };
            var transitionFunction =
                new TransitionFunction(compoundActions, states)
                {
                    [caPeek, s0] = new HashSet<State>() {s0,s1,s2},
                    [caPet, s0] = new HashSet<State>() {s0},
                    [caPeekPet, s0] = new HashSet<State>() {s0,s1,s2},
                    [caPeek, s1] = new HashSet<State>() {s1,s2},
                    [caPet, s1] = new HashSet<State>() {s0},
                    [caPeekPet, s1] = new HashSet<State>() {s1,s2},
                    [caPeek, s2] = new HashSet<State>() {s2},
                    [caPet, s2] = new HashSet<State>() {s2},
                    [caPeekPet, s2] = new HashSet<State>() {s2},
                };
            models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();

            queries = new QuerySet();
            queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Negation(new Literal(alive)),
                    new Program(new List<CompoundAction>() { caPet, caPeek })
                    ));
            queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Literal(purring), 
                    new Program(new List<CompoundAction>() { caPeekPet })
                    ));
            queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPeek, caPet, caPeekPet })
                    ));
            queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caPeek, caPet, caPeekPet })
                    ));
            queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Literal(purring)
                    ));
        }

        [Test]
        public void SchrodingerCatExistentialExecutability()
        {
            // given
            BuildSchrodingerCat(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(models, queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatGeneralExecutability()
        {
            BuildSchrodingerCat(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(models, queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatExistentialValue()
        {
            BuildSchrodingerCat(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(models, queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void SchrodingerCatGeneralValue()
        {
            BuildSchrodingerCat(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(models, queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void SchrodingerCatAccessibility()
        {
            BuildSchrodingerCat(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(models, queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }


        
        public void BuildTwoPainters(out ICollection<Structure> models, out QuerySet queries)
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
                    [caPaint, s0] = new HashSet<State>() {s0},
                    [caTakeA, s0] = new HashSet<State>() {s1},
                    [caTakeB, s0] = new HashSet<State>() {s2},
                    [caTakeATakeB, s0] = new HashSet<State>() {s1, s2},
                    [caTakeAPaint, s0] = new HashSet<State>() {s1},
                    [caTakeBPaint, s0] = new HashSet<State>() {s2},
                    [caTakeATakeBPaint, s0] = new HashSet<State>() {s1, s2},
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
            models = initialStates.Select(state => new Structure(states, state, transitionFunction)).ToList();
            

            queries = new QuerySet();
            queries.ExistentialValueQueries.Add(
                new ExistentialValueQuery(
                    new Literal(brushA),
                    new Program(new List<CompoundAction>() { caTakeATakeB })
                    ));
            queries.GeneralValueQueries.Add(
                new GeneralValueQuery(
                    new Conjunction(new Negation(new Literal(brushA)), new Negation(new Literal(brushB))),
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeBPaint })
                    ));
            queries.ExistentialExecutabilityQueries.Add(
                new ExistentialExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeB, caTakeA })
                    ));
            queries.GeneralExecutabilityQueries.Add(
                new GeneralExecutabilityQuery(
                    new Program(new List<CompoundAction>() { caTakeATakeB, caTakeATakeB, caTakeBPaint })
                    ));
            queries.AccessibilityQueries.Add(
                new AccessibilityQuery(
                    new Conjunction(new Literal(brushA), new Literal(brushB))
                    ));
        }

        [Test]
        public void TwoPaintersExistentialExecutability()
        {
            BuildTwoPainters(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialExecutabilityQuery(models, queries.ExistentialExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersGeneralExecutability()
        {
            BuildTwoPainters(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralExecutabilityQuery(models, queries.GeneralExecutabilityQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersExistentialValue()
        {
            BuildTwoPainters(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateExistentialValueQuery(models, queries.ExistentialValueQueries[0]);
            //check
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TwoPaintersGeneralValue()
        {
            BuildTwoPainters(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateGeneralValueQuery(models, queries.GeneralValueQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }

        [Test]
        public void TwoPaintersAccessibility()
        {
            BuildTwoPainters(out var models, out QuerySet queries);
            //do
            bool result = QueriesEvaluator.EvaluateAccessibilityQuery(models, queries.AccessibilityQueries[0]);
            //check
            Assert.AreEqual(false, result);
        }
    }
}
