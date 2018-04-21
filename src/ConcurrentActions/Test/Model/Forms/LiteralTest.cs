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
    }
}