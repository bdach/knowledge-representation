using System;
using FluentAssertions;
using Model;
using NUnit.Framework;
using Action = Model.Action;

namespace Test.Model
{
    [TestFixture]
    public class CompoundActionTest
    {
        [Test]
        public void Constructor_SameLength()
        {
            // given
            var actions = new[] {new Action("Put"), new Action("Peek")};
            var selection = new[] {true, false};
            // when
            var compoundAction = new CompoundAction(actions, selection);
            // then
            compoundAction.Actions.Should().Contain(actions[0]);
            compoundAction.Actions.Should().NotContain(actions[1]);
        }

        [Test]
        public void Constructor_MoreActionsThanSelections()
        {
            // given
            var actions = new[] {new Action("Put"), new Action("Peek"), new Action("Consume")};
            var selection = new[] {true, false};
            // when
            var createCompoundAction = new System.Action(() => new CompoundAction(actions, selection));
            // then
            createCompoundAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Constructor_MoreSelectionsThanActions()
        {
            // given
            var actions = new[] {new Action("Put"), new Action("Peek")};
            var selection = new[] {true, false, true};
            // when
            var createCompoundAction = new System.Action(() => new CompoundAction(actions, selection));
            // then
            createCompoundAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void AsString()
        {
            // given
            var actions = new[] {new Action("Put"), new Action("Peek")};
            var compoundAction = new CompoundAction(actions);
            // when
            var asString = compoundAction.ToString();
            // then
            asString.Should().Be("{Put, Peek}");
        }
    }
}