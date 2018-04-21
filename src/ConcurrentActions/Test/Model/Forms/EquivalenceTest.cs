using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class EquivalenceTest
    {
        [Test, Sequential]
        public void Evaluate([Values(false, false, true, true)] bool leftValue,
            [Values(false, true, false, true)] bool rightValue,
            [Values(true, false, false, true)] bool expectedValue)
        {
            // given
            var state = new Mock<IState>();
            var left = new Mock<IFormula>();
            left.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(leftValue);
            var right = new Mock<IFormula>();
            right.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(rightValue);
            var equivalence = new Equivalence(left.Object, right.Object);
            // when
            var value = equivalence.Evaluate(state.Object);
            // then
            value.Should().Be(expectedValue);
        }
    }
}