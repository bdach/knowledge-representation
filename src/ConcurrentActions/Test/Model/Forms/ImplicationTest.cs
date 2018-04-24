using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class ImplicationTest
    {
        [Test, Sequential]
        public void Evaluate([Values(false, false, true, true)] bool antecedentValue,
            [Values(false, true, false, true)] bool consequentValue,
            [Values(true, true, false, true)] bool expectedValue)
        {
            // given
            var state = new Mock<IState>();
            var antecedent = new Mock<IFormula>();
            antecedent.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(antecedentValue);
            var consequent = new Mock<IFormula>();
            consequent.Setup(f => f.Evaluate(It.IsAny<IState>())).Returns(consequentValue);
            var implication = new Implication(antecedent.Object, consequent.Object);
            // when
            var value = implication.Evaluate(state.Object);
            // then
            value.Should().Be(expectedValue);
        }
    }
}