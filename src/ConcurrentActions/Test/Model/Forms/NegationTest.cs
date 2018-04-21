using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class NegationTest
    {
        [Test]
        public void Evaluate([Values(true, false)] bool innerValue)
        {
            // given
            var formula = new Mock<IFormula>();
            var state = new Mock<IState>();
            formula.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(innerValue);
            var negation = new Negation(formula.Object);
            // when
            var value = negation.Evaluate(state.Object);
            // then
            value.Should().Be(!innerValue);
        }
    }
}