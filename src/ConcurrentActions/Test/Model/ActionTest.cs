using FluentAssertions;
using Model;
using NUnit.Framework;

namespace Test.Model
{
    [TestFixture]
    public class ActionTest
    {
        public void Equal()
        {
            // given
            var action1 = new Action("Peek");
            var action2 = new Action("Peek");
            // when
            var equal = action1.Equals(action2);
            // then
            equal.Should().BeTrue();
        }

        public void NotEqual()
        {
            // given
            var action1 = new Action("Action1");
            var action2 = new Action("Action2");
            // when
            var equal = action1.Equals(action2);
            // then
            equal.Should().BeFalse();
        }
    }
}