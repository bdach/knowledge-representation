using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF;
using FluentAssertions;
using FluentAssertions.Common;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.DNF
{
    [TestFixture]
    public class DnfFormulaTest
    {
        [Test()]
        public void TestConflictWhenTrue()
        {
            // given
            var formulaA = new DnfFormula(null,
                new List<NaryConjunction>()
                {
                    new NaryConjunction(new List<Literal>(), new List<Constant>() {Constant.Truth})
                });
            var formulaB = new DnfFormula(null,
                new List<NaryConjunction>() {new NaryConjunction(new List<Literal>(), new List<Constant>())});
            // when
            var value = formulaA.Conflicts(formulaB);
            // then
            value.Should().Be(false);
        }

        [Test()]
        public void TestConflictWhenFalse()
        {
            // given
            var formulaA = new DnfFormula(null,
                new List<NaryConjunction>()
                {
                    new NaryConjunction(new List<Literal>(), new List<Constant>() {Constant.Falsity})
                });
            var formulaB = new DnfFormula(null,
                new List<NaryConjunction>() {new NaryConjunction(new List<Literal>(), new List<Constant>())});
            // when
            var value = formulaA.Conflicts(formulaB);
            // then
            value.Should().Be(true);
        }

        [Test()]
        public void TestAccept()
        {
            // given
            var formula = new Mock<IFormula>();
            var dnf = new DnfFormula(formula.Object, new List<NaryConjunction>());
            var visitor = new Mock<IFormulaVisitor>();
            // when
            dnf.Accept(visitor.Object);
            // then
            formula.Verify(f => f.Accept(visitor.Object));
        }


        [Test()]
        public void TestEvaluate()
        {
            // given
            var formula = new Mock<IFormula>();
            var state = new Mock<IState>();
            formula.Setup(f => f.Evaluate(state.Object)).Returns(true);
            var dnf = new DnfFormula(formula.Object, new List<NaryConjunction>());
            // when
            var result = dnf.Evaluate(state.Object);
            // then
            formula.Verify(f => f.Evaluate(state.Object));
            result.IsSameOrEqualTo(true);
        }
    }
}