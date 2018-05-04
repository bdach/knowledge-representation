using System.Collections.Generic;
using System.Linq;
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
        private readonly Fluent _bufferEmpty = new Fluent("bufferEmpty");
        private readonly Fluent _hasItem = new Fluent("hasItem");

        private readonly Action _put = new Action("put");
        private readonly Action _get = new Action("get");
        private readonly Action _consume = new Action("consume");

        private static State _stateZero;
        private State _stateOne;
        private State _stateTwo;
        private State _stateThree;

        private ActionDomain ProducerConsumerActionDomain()
        {
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(_put, new Literal(_bufferEmpty),
                new Negation(new Literal(_bufferEmpty))));
            actionDomain.EffectStatements.Add(new EffectStatement(_get,
                new Conjunction(new Negation(new Literal(_bufferEmpty)), new Negation(new Literal(_hasItem))),
                new Conjunction(new Literal(_bufferEmpty), new Literal(_hasItem))));
            actionDomain.EffectStatements.Add(new EffectStatement(_consume, new Negation(new Literal(_hasItem))));
            actionDomain.EffectStatements.Add(EffectStatement.Impossible(_consume, new Literal(_hasItem, true)));
            return actionDomain;
        }

        private Structure ProducerConsumerStructure()
        {
            var fluentInitZero = new Dictionary<Fluent, bool>() {{_bufferEmpty, true}, {_hasItem, true}};
            var fluentStateOne = new Dictionary<Fluent, bool>() {{_bufferEmpty, false}, {_hasItem, true}};
            var fluentStatesTwo = new Dictionary<Fluent, bool>() {{_bufferEmpty, true}, {_hasItem, false}};
            var fluentStatesThree = new Dictionary<Fluent, bool>() {{_bufferEmpty, false}, {_hasItem, false}};

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

        private DynamicSystem.ResZero.ResZero CreateResZeroObjInstance()
        {
            var domain = ProducerConsumerActionDomain();
            var structure = ProducerConsumerStructure();
            return new DynamicSystem.ResZero.ResZero(domain.EffectStatements, structure.States);
        }

        [Test]
        public void ResZero_PUT_Test()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() {_put});
            var resultStates = new List<State>() {_stateOne, _stateThree};
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero,compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_GET_Test()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() {_get});
            var resultStates = new List<State>() {_stateZero, _stateOne, _stateTwo,_stateThree};
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_CONSUME_Test()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() { _consume });
            var resultStates = new List<State>() { _stateTwo,_stateThree };
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_GET_PUT_Test()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() {_get, _put});
            var resultStates = new List<State>() { _stateOne, _stateThree };
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZero_PUT_CONSUME_ImpossibilityTest()
        {
            // given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action> {_put, _consume});
            // when
            var resZeroStates = resTestObjInstance.GetStates(_stateTwo, compoundAction);
            // then
            resZeroStates.Should().BeEmpty();
        }

        [Test]
        public void ResZero_CONSUME_ImpossibilityTest()
        {
            // given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action> {_consume});
            // when
            var resZeroStates = resTestObjInstance.GetStates(_stateTwo, compoundAction);
            // then
            resZeroStates.Should().BeEmpty();
        }
    }
}