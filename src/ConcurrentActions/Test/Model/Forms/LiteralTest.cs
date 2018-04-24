using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class LiteralTest
    {
        [Test, Sequential]
        public void Evaluate([Values(true, false, true, false)] bool fluentValue,
            [Values(false, false, true, true)] bool negated,
            [Values(true, false, false, true)] bool expectedResult)
        {
            // given
            var fluent = new Fluent("test");
            var literal = new Literal(fluent, negated);
            var state = new Mock<IState>();
            state.Setup(s => s[fluent]).Returns(fluentValue);
            // when
            var value = literal.Evaluate(state.Object);
            // then
            value.Should().Be(expectedResult);
        }

        [Test]
        public void Equal()
        {
            // given
            var literal1 = new Literal(new Fluent("test"), true);
            var literal2 = new Literal(new Fluent("test"), true);
            // when
            var equal = literal1.Equals(literal2);
            // then
            equal.Should().BeTrue();
        }

        [Test]
        public void NotEqual_SameFluent()
        {
            // given
            var fluent = new Fluent("test");
            var literal1 = new Literal(fluent, true);
            var literal2 = new Literal(fluent, false);
            // when
            var equal = literal1.Equals(literal2);
            // then
            equal.Should().BeFalse();
        }

        [Test]
        public void NotEqual_DifferentFluents()
        {
            // given
            var literal1 = new Literal(new Fluent("test1"));
            var literal2 = new Literal(new Fluent("test2"));
            // when
            var equal = literal1.Equals(literal2);
            // then
            equal.Should().BeFalse();
        }
    }
}