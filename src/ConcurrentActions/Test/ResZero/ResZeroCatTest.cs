using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;
using Action = Model.Action;

namespace Test.ResZero
{
    internal class ResZeroCatTest
    {

        private readonly Fluent _alive = new Fluent("alive");
        private readonly Fluent _purring = new Fluent("purring");

        private readonly Action _peek = new Action("peek");
        private readonly Action _pet = new Action("pet");

        private static State _stateZero;
        private State _stateOne;
        private State _stateTwo;

        private ActionDomain CatActionDomain()
        {
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(_pet, new Literal(_alive), new Literal(_purring)));
            return actionDomain;
        }

        private Structure CatStructure()
        {
            var fluentInitZero = new Dictionary<Fluent, bool>() { { _alive, true }, { _purring, true } };
            var fluentStateOne = new Dictionary<Fluent, bool>() { { _alive, true }, { _purring, false } };
            var fluentStatesTwo = new Dictionary<Fluent, bool>() { { _alive, false }, { _purring, false } };

            _stateZero =  new State(fluentInitZero);
            _stateOne = new State(fluentStateOne);
            _stateTwo = new State(fluentStatesTwo);
            var allStates = new List<State>
                {
                    _stateZero,
                    _stateOne,
                    _stateTwo
                };

            return new Structure(allStates, _stateZero, null);
        }

        [Test]
        public void ResZeroCatPeekTest()
        {
            //given
            var actionDomain = CatActionDomain();
            var structure = CatStructure();
            var compoundAction = new CompoundAction(new List<Action>() { _peek });
            var resultStates = new List<State>() {_stateTwo};
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, _stateTwo, actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZeroCatPetTest()
        {
            //given
            var actionDomain = CatActionDomain();
            var structure = CatStructure();
            var compoundAction = new CompoundAction(new List<Action>() { _pet });
            var resultStates = new List<State>() {_stateZero };
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState, actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }
        
    }
}


