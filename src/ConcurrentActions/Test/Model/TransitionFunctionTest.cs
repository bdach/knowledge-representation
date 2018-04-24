using System.Collections.Generic;
using FluentAssertions;
using Model;
using NUnit.Framework;

namespace Test.Model
{
    [TestFixture]
    public class TransitionFunctionTest
    {
        [Test]
        public void Accessor_Getter()
        {
            // given
            var fluents = new[] {new Fluent("test1"), new Fluent("test2")};
            var values = new[] {true, false};
            var state = new State(fluents, values);
            var states = new[] {state};
            var action = new Action("test");
            var compoundAction = new CompoundAction(action);
            var compoundActions = new[] {compoundAction};
            var transitionFunction = new TransitionFunction(compoundActions, states);
            // when
            transitionFunction[compoundAction, state].Add(state);
            // then
            transitionFunction[compoundAction, state].Should().Contain(state);
        }

        [Test]
        public void Accessor_Setter()
        {
            // given
            var fluents = new[] { new Fluent("test1"), new Fluent("test2") };
            var values = new[] { true, false };
            var state = new State(fluents, values);
            var states = new[] { state };
            var action = new Action("test");
            var compoundAction = new CompoundAction(action);
            var compoundActions = new[] { compoundAction };
            var transitionFunction = new TransitionFunction(compoundActions, states);
            // when
            transitionFunction[compoundAction, state] = new HashSet<State> {state};
            // then
            transitionFunction[compoundAction, state].Should().Contain(state);
        }
    }
}