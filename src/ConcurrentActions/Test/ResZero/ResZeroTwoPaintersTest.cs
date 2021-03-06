﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;
using Action = Model.Action;

namespace Test.ResZero
{
    [TestFixture]
    internal class ResZeroTwoPaintersTest
    {
        private readonly Fluent _brushA = new Fluent("brushA");
        private readonly Fluent _brushB = new Fluent("brushB");

        private readonly Action _takeA = new Action("TakeA");
        private readonly Action _takeB = new Action("TakeB");
        private readonly Action _paint = new Action("Paint");

        private State _stateZero;
        private State _stateOne;
        private State _stateTwo;

        private ActionDomain TwoPaintersActionDomain()
        {
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(_takeA,
                new Conjunction(new Literal(_brushA), new Negation(new Literal(_brushB)))));
            actionDomain.EffectStatements.Add(new EffectStatement(_takeB,
                new Conjunction(new Negation(new Literal(_brushA)), new Literal(_brushB))));
            actionDomain.EffectStatements.Add(new EffectStatement(_paint, new Literal(_brushA),
                new Negation(new Literal(_brushA))));
            actionDomain.EffectStatements.Add(new EffectStatement(_paint, new Literal(_brushB),
                new Negation(new Literal(_brushB))));
            return actionDomain;
        }

        private Structure TwoPaintersStructure()
        {
            var fluentInitZero = new Dictionary<Fluent, bool> {{_brushA, false}, {_brushB, false}};
            var fluentStateOne = new Dictionary<Fluent, bool> {{_brushA, true}, {_brushB, false}};
            var fluentStatesTwo = new Dictionary<Fluent, bool> {{_brushA, false}, {_brushB, true}};

            _stateZero = new State(fluentInitZero);
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


        private DynamicSystem.ResZero.ResZero CreateResZeroObjInstance()
        {
            var domain = TwoPaintersActionDomain();
            var structure = TwoPaintersStructure();
            return new DynamicSystem.ResZero.ResZero(domain.EffectStatements, structure.States);
        }


        [Test]
        public void ResZeroTwoPaintersTakeATest()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() {_takeA});
            var resultStates = new List<State>() {_stateOne};
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction.Actions);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }


        [Test]
        public void ResZeroTwoPaintersTakeBTest()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action> {_takeB});
            var resultStates = new List<State> {_stateTwo};
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction.Actions);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }

        [Test]
        public void ResZeroTwoPaintersPaintTest()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action>() {_paint});
            var resultStates = new List<State>() {_stateZero,_stateOne,_stateTwo};
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction.Actions);
            //then
            var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
            sequenceEqual.Should().BeTrue();
        }


        [Test]
        public void ResZeroTwoPaintersCompoundActionTest()
        {
            //given
            var resTestObjInstance = CreateResZeroObjInstance();
            var compoundAction = new CompoundAction(new List<Action> {_takeA, _takeB});
            //when
            var resZeroStates = resTestObjInstance.GetStates(_stateZero, compoundAction.Actions);
            //then
            resZeroStates.Should().BeEmpty();
            // I know that this might seem counterintuitive that we're expecting an empty set of result states here,
            // but take note that the Res_0 calculation object does not know anything about action decompositions at this stage.
            // Hence we should be getting an empty set here, since the two actions have inconsistent postconditions.
        }
    }
}