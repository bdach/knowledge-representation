using System;
using FluentAssertions;
using Model;
using NUnit.Framework;
using Action = System.Action;

namespace Test.Model
{
    [TestFixture]
    public class StateTest
    {
        [Test]
        public void Constructor_SameLength()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var values = new[] {true, false};
            // when
            var state = new State(fluents, values);
            // then
            state[fluents[0]].Should().BeTrue();
            state[fluents[1]].Should().BeFalse();
        }

        [Test]
        public void Constructor_MoreValuesThanFluents()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var values = new[] {true, false, true};
            // when
            var createState = new Action(() => new State(fluents, values));
            // then
            createState.Should().Throw<ArgumentException>("the value array is longer than the fluent array");
        }

        [Test]
        public void Constructor_MoreFluentsThanValues()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB"), new Fluent("hasC")};
            var values = new[] {true, false};
            // when
            var createState = new Action(() => new State(fluents, values));
            // then
            createState.Should().Throw<ArgumentException>("the fluent array is longer than the value array");
        }

        [Test]
        public void Accessor_GetValue()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var values = new[] {true, false};
            // when
            var state = new State(fluents, values);
            var value = state[fluents[0]];
            // then
            value.Should().BeTrue();
        }

        [Test]
        public void Accessor_GetValue_NonExistentFluent()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var otherFluent = new Fluent("test");
            var values = new[] {true, false};
            // when
            var state = new State(fluents, values);
            var getState = new Action(() =>
            {
                var _ = state[otherFluent];
            });
            // then
            getState.Should().Throw<ArgumentException>("the supplied fluent is not specified in the state");
        }

        [Test]
        public void Accessor_SetValue()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var values = new[] {true, false};
            // when
            var state = new State(fluents, values);
            state[fluents[0]] = false;
            var value = state[fluents[0]];
            // then
            value.Should().BeFalse();
        }

        [Test]
        public void Accessor_SetValue_NonExistentFluent()
        {
            // given
            var fluents = new[] {new Fluent("hasA"), new Fluent("hasB")};
            var otherFluent = new Fluent("test");
            var values = new[] {true, false};
            // when
            var state = new State(fluents, values);
            var setState = new Action(() => { state[otherFluent] = true; });
            // then
            setState.Should().Throw<ArgumentException>("the supplied fluent is not specified in the state");
        }
    }
}