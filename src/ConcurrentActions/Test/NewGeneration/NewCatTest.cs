using NUnit.Framework;
using Model;
using Action = Model.Action;
using Model.ActionLanguage;
using System.Collections.Generic;
using Model.Forms;
using FluentAssertions;
using DynamicSystem.NewGeneration;
using System.Linq;

namespace Test.NewGeneration
{
    [TestFixture]
    public class NewCatTest
    {
        private readonly Fluent _alive = new Fluent("alive");
        private readonly Fluent _purring = new Fluent("purring");
        private HashSet<Fluent> _fluents;
        private ActionDomain _actionDomain;

        private readonly Action _peek = new Action("peek");
        private readonly Action _pet = new Action("pet");

        private static State _stateZero;
        private State _stateOne;
        private State _stateTwo;


        [OneTimeSetUp]
        public void Init()
        {
            _actionDomain = new ActionDomain();
            _actionDomain.EffectStatements.Add(new EffectStatement(_pet, new Literal(_alive), new Literal(_purring)));
            _actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(_peek, _alive, new Literal(_alive)));
            _actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(_peek, _purring, new Literal(_alive)));
            _actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Implication
                                                                                (
                                                                                 new Negation(new Literal(_alive)),
                                                                                 new Negation(new Literal(_purring))
                                                                                )
                                                                            ));

            _fluents = new HashSet<Fluent> { _alive, _purring };


            var fluentInitZero = new Dictionary<Fluent, bool>() { { _alive, true }, { _purring, true } };
            var fluentStateOne = new Dictionary<Fluent, bool>() { { _alive, true }, { _purring, false } };
            var fluentStatesTwo = new Dictionary<Fluent, bool>() { { _alive, false }, { _purring, false } };

            _stateZero = new State(fluentInitZero);
            _stateOne = new State(fluentStateOne);
            _stateTwo = new State(fluentStatesTwo);
        }

        [Test]
        public void NewCatS0ToS0()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var literals = newGenerator.GetLiterals(_peek, _stateZero, _stateZero);

            Assert.IsTrue(literals.Contains(new Literal(_alive, false)));
            Assert.IsTrue(literals.Contains(new Literal(_purring, false)));
        }

        [Test]
        public void NewCatS0ToS1()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var literals = newGenerator.GetLiterals(_peek, _stateZero, _stateOne);

            Assert.IsTrue(literals.Contains(new Literal(_alive, false)));
            Assert.IsTrue(literals.Contains(new Literal(_purring, true)));
        }

        [Test]
        public void NewCatS0ToS2()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var literals = newGenerator.GetLiterals(_peek, _stateZero, _stateTwo);

            Assert.IsTrue(literals.Contains(new Literal(_alive, true)));
            Assert.IsTrue(literals.Contains(new Literal(_purring, true)));
        }

        [Test]
        public void NewCatS1ToS2CompoundAction()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var newFluentsForPeek = newGenerator.GetLiterals(_peek, _stateOne, _stateTwo);
            var newFluentsForPet = newGenerator.GetLiterals(_pet, _stateOne, _stateTwo);

            //should just join them here
            var outLiterals = newFluentsForPeek.Union(newFluentsForPet);
            Assert.AreEqual(2, outLiterals.Count());
            Assert.IsTrue(outLiterals.Contains(new Literal(_alive, true)));
            Assert.IsTrue(outLiterals.Contains(new Literal(_purring, true)));
        }

        [Test]
        public void NewCatS1ToS1CompoundAction()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var actions = new List<Action>() { _peek, _pet};

            var outLiterals = newGenerator.GetLiterals(actions, _stateOne, _stateOne);
          
            Assert.AreEqual(2, outLiterals.Count());
            Assert.IsTrue(outLiterals.Contains(new Literal(_alive, false)));
            Assert.IsTrue(outLiterals.Contains(new Literal(_purring, true)));
        }

        [Test]
        public void NewCatS2ToS2CompoundAction()
        {
            var newGenerator = new NewSetHelper(_actionDomain, _fluents);
            var actions = new List<Action>() { _peek, _pet };

            var outLiterals = newGenerator.GetLiterals(actions, _stateTwo, _stateTwo);
            Assert.AreEqual(0, outLiterals.Count());
        }
    }
}
