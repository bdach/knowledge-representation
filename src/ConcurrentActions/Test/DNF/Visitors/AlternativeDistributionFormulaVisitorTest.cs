using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Test.DNF.Visitors
{
    [TestFixture]
    public class AlternativeDistributionFormulaVisitorTest
    {
        [Test()]
        public void TestVisitConjunctionWithNoAlternatives()
        {
            // given
            var visitor = new AlternativeDistributionFormulaVisitor();
            var (left, leftResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var (right, rightResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var conjunction = new Conjunction(left, right);
            var expected = new Conjunction(leftResult, rightResult);
            // when
            var result = visitor.Visit(conjunction);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitConjunctionWithLeftAlternative()
        {
            // given
            var visitor = new AlternativeDistributionFormulaVisitor();
            var a = new Literal(new Fluent("a"));
            var b = new Literal(new Fluent("b"));
            var c = new Literal(new Fluent("c"));
            var conjunction = new Conjunction(
                new Alternative(a, b),
                c
            );
            var expected = new Alternative(
                new Conjunction(c, a),
                new Conjunction(c, b)
            );
            // when
            var result = visitor.Visit(conjunction);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitConjunctionWithRightAlternative()
        {
            // given
            var visitor = new AlternativeDistributionFormulaVisitor();
            var a = new Literal(new Fluent("a"));
            var b = new Literal(new Fluent("b"));
            var c = new Literal(new Fluent("c"));
            var conjunction = new Conjunction(
                a,
                new Alternative(b, c)
            );
            var expected = new Alternative(
                new Conjunction(a, b),
                new Conjunction(a, c)
            );
            // when
            var result = visitor.Visit(conjunction);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitConjunctionWithBothAlternatives()
        {
            // given
            var visitor = new AlternativeDistributionFormulaVisitor();
            var a = new Literal(new Fluent("a"));
            var b = new Literal(new Fluent("b"));
            var c = new Literal(new Fluent("c"));
            var d = new Literal(new Fluent("d"));
            var conjunction = new Conjunction(
                new Alternative(a, b),
                new Alternative(c, d)
            );
            var expected = new Alternative(
                new Alternative(
                    new Conjunction(a, c),
                    new Conjunction(a, d)
                ),
                new Alternative(
                    new Conjunction(b, c),
                    new Conjunction(b, d)
                )
            );
            // when
            var result = visitor.Visit(conjunction);
            // then
            Assert.AreEqual(expected, result);
        }


        [Test()]
        public void TestVisitAlternative()
        {
            // given
            var visitor = new AlternativeDistributionFormulaVisitor();
            var (left, leftResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var (right, rightResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var alternative = new Alternative(left, right);
            var expected = new Alternative(leftResult, rightResult);
            // when
            var result = visitor.Visit(alternative);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitLiteral()
        {
            var literal = new Literal(new Fluent("fluent"));
            NUnit.Framework.Assert.AreSame(new AlternativeDistributionFormulaVisitor().Visit(literal), literal);
        }

        [Test()]
        public void TestVisitConstant()
        {
            NUnit.Framework.Assert.AreEqual(new AlternativeDistributionFormulaVisitor().Visit(Constant.Truth),
                Constant.Truth);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        public void TestVisitImplication()
        {
            new AlternativeDistributionFormulaVisitor().Visit(new Mock<Implication>().Object);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        public void TestVisitEquivalence()
        {
            new AlternativeDistributionFormulaVisitor().Visit(new Mock<Equivalence>().Object);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        public void TestVisitNegation()
        {
            new AlternativeDistributionFormulaVisitor().Visit(new Mock<Negation>().Object);
        }
    }
}