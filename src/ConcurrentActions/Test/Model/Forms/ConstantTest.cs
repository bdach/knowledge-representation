using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.Model.Forms
{
    [TestFixture]
    public class ConstantTest
    {
        [Test]
        public void Truth()
        {
            // given
            var state = new Mock<IState>();
            var truth = Constant.Truth;
            // when
            var value = truth.Evaluate(state.Object);
            // then
            value.Should().BeTrue();
        }

        [Test]
        public void Falsity()
        {
            // given
            var state = new Mock<IState>();
            var falsity = Constant.Falsity;
            // when
            var value = falsity.Evaluate(state.Object);
            // then
            value.Should().BeFalse();
        }
    }
}