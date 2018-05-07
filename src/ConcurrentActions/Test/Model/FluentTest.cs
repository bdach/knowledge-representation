using FluentAssertions;
using Model;
using NUnit.Framework;

namespace Test.Model
{
    [TestFixture]
    public class FluentTest
    {
        [Test]
        public void Equal()
        {
            // given
            var fluent1 = new Fluent("test");
            var fluent2 = new Fluent("test");
            // when
            var equal = fluent1.Equals(fluent2);
            // then
            equal.Should().BeTrue();
        }

        [Test]
        public void NotEqual()
        {
            // given
            var fluent1 = new Fluent("hasA");
            var fluent2 = new Fluent("hasB");
            // when
            var equal = fluent1.Equals(fluent2);
            // then
            equal.Should().BeFalse();
        }
    }
}