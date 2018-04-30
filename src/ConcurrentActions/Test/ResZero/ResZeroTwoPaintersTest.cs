using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.SetGeneration;
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

            private  State _stateZero;
            private  State _stateOne;
            private  State _stateTwo;

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
                var fluentInitZero = new Dictionary<Fluent, bool>() {{_brushA, false}, {_brushB, false}};
                var fluentStateOne = new Dictionary<Fluent, bool>() {{_brushA, true}, {_brushB, false}};
                var fluentStatesTwo = new Dictionary<Fluent, bool>() {{_brushA, false}, {_brushB, true}};

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

            
            [Test]
            public void ResZeroTwoPaintersTakeATest()
            {
                //given
                var actionDomain = TwoPaintersActionDomain();
                var structure = TwoPaintersStructure();
                var compoundAction = new CompoundAction(new List<Action>() { _takeA});
                var resultStates = new List<State>() { _stateOne };
                //when
                var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States,_stateOne, actionDomain, compoundAction);
                //then
                var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
                sequenceEqual.Should().BeTrue();
            }


            [Test]
            public void ResZeroTwoPaintersTakeBTest()
            {
                //given
                var actionDomain = TwoPaintersActionDomain();
                var structure = TwoPaintersStructure();
                var compoundAction = new CompoundAction(new List<Action>() { _takeB });
                var resultStates = new List<State>() { _stateTwo };
                //when
                var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, _stateZero, actionDomain, compoundAction);
                //then
                var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
                sequenceEqual.Should().BeTrue();
            }

            [Test]
            public void ResZeroTwoPaintersPaintTest()
            {
                //given
                var actionDomain = TwoPaintersActionDomain();
                var structure = TwoPaintersStructure();
                var initialState = structure.InitialState;
                var compoundAction = new CompoundAction(new List<Action>() { _paint });
                var resultStates = new List<State>() { initialState };
                //when
                var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, _stateZero, actionDomain, compoundAction);
                //then
                var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
                sequenceEqual.Should().BeTrue();
            }


            [Test]
            public void ResZeroTwoPaintersCompoundActionTest()
            {
                //given
                var actionDomain = TwoPaintersActionDomain();
                var structure = TwoPaintersStructure();
                var compoundAction = new CompoundAction(new List<Action>(){_takeA,_takeB});
                var resultStates = new List<State>() {_stateOne, _stateTwo};
                //when
                var resZeroStates = DynamicSystem.ResZero.ResZero.GetStates(structure.States, structure.InitialState, actionDomain, compoundAction);
                //then
                var sequenceEqual = resZeroStates.SequenceEqual(resultStates);
                sequenceEqual.Should().BeTrue();
            }

        }
    
}
    