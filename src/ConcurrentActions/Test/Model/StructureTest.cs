using System;
using FluentAssertions;
using Model;
using NUnit.Framework;

namespace Test.Model
{
    [TestFixture]
    public class StructureTest
    {
        [Test]
        public void Constructor()
        {
            // given
            var fluents = new[] {new Fluent("test"), new Fluent("test2")};
            var values1 = new[] {true, true};
            var values2 = new[] {false, true};
            var state1 = new State(fluents, values1);
            var state2 = new State(fluents, values2);
            var states = new[] {state1, state2};
            // when
            var createStructure = new System.Action(() => new Structure(states, state1, null)); // I'm sorry Tony Hoare
            // then
            createStructure.Should().NotThrow();
        }

        [Test]
        public void Constructor_InvalidIniitialState()
        {
            // given
            var fluents = new[] { new Fluent("test"), new Fluent("test2") };
            var values1 = new[] { true, true };
            var values2 = new[] { false, true };
            var values3 = new[] { true, true };
            var state1 = new State(fluents, values1);
            var state2 = new State(fluents, values2);
            var state3 = new State(fluents, values3);
            var states = new[] { state1, state2 };
            // when
            var createStructure = new System.Action(() => new Structure(states, state3, null)); // I'm sorry Tony Hoare
            // then
            createStructure.Should().Throw<ArgumentException>();
        }
    }
}