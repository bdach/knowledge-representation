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
    internal class ProducentConsumerTest
    {
        private readonly Fluent bufferEmpty = new Fluent("bufferEmpty");
        private readonly Fluent hasItem = new Fluent("hasItem");

        private readonly Action put = new Action("put");
        private readonly Action get = new Action("get");
        private readonly Action consume = new Action("consume");

        private static State _stateZero;
        private State _stateOne;
        private State _stateTwo;
        private State _stateThree;

        private ActionDomain ProducerConsumerActionDomain()
        {
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(put, new Literal(bufferEmpty),
                new Negation(new Literal(bufferEmpty))));
            actionDomain.EffectStatements.Add(new EffectStatement(get,
                new Conjunction(new Negation(new Literal(bufferEmpty)), new Negation(new Literal(hasItem))),
                new Conjunction(new Literal(bufferEmpty), new Literal(hasItem))));
            actionDomain.EffectStatements.Add(new EffectStatement(consume, new Negation(new Literal(hasItem))));
            return actionDomain;
        }

        private Structure ProducerConsumerStructure()
        {
            var fluentInitZero = new Dictionary<Fluent, bool>() {{bufferEmpty, true}, {hasItem, true}};
            var fluentStateOne = new Dictionary<Fluent, bool>() {{bufferEmpty, false}, {hasItem, true}};
            var fluentStatesTwo = new Dictionary<Fluent, bool>() {{bufferEmpty, true}, {hasItem, false}};
            var fluentStatesThree = new Dictionary<Fluent, bool>() {{bufferEmpty, false}, {hasItem, false}};

            _stateZero = new State(fluentInitZero);
            _stateOne = new State(fluentStateOne);
            _stateTwo = new State(fluentStatesTwo);
            _stateThree = new State(fluentStatesThree);
            var allStates = new List<State>
            {
                _stateZero,
                _stateOne,
                _stateTwo,
                _stateThree
            };

            return new Structure(allStates, _stateZero, null);
        }

        [Test]
        public void ResZero_PUT_Test()
        {
            //given
            var actionDomain = ProducerConsumerActionDomain();
            var structure = ProducerConsumerStructure();
            var compoundAction = new CompoundAction(new List<Action>() {put});
            var resultStates = new List<State>() {_stateOne};
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState,
                actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_GET_Test()
        {
            //given
            var actionDomain = ProducerConsumerActionDomain();
            var structure = ProducerConsumerStructure();
            var compoundAction = new CompoundAction(new List<Action>() {get});
            var resultStates = new List<State>() {_stateZero};
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState,
                actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_CONSUME_Test()
        {
            //given
            var actionDomain = ProducerConsumerActionDomain();
            var structure = ProducerConsumerStructure();
            var compoundAction = new CompoundAction(new List<Action>() { consume });
            var resultStates = new List<State>() { _stateTwo };
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState,
                actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }




        [Test]
        public void ResZero_PUT_GET_Test()
        {
            //given
            var actionDomain = ProducerConsumerActionDomain();
            var structure = ProducerConsumerStructure();
            var compoundAction = new CompoundAction(new List<Action>() {put, get});
            var resultStates = new List<State>() {_stateOne};
            //when
            var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState,
                actionDomain, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }
    }
}