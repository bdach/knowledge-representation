using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class ConjunctionTest
    {
        [Test, Sequential]
        public void Evaluate([Values(false, true, false, true)] bool leftValue,
            [Values(false, false, true, true)] bool rightValue,
            [Values(false, false, false, true)] bool expectedValue)
        {
            // given
            var state = new Mock<IState>();
            var left = new Mock<IFormula>();
            left.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(leftValue);
            var right = new Mock<IFormula>();
            right.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(rightValue);
            var conjunction = new Conjunction(left.Object, right.Object);
            // when
            var value = conjunction.Evaluate(state.Object);
            // then
            value.Should().Be(expectedValue);
        }
    }
}