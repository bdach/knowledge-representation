using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF.Visitors;
using FluentAssertions.Common;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.DNF.Visitors
{
    [TestFixture]
    public class SimplifyingFormulaVisitorTest
    {
        [Test()]
        public void TestVisitConjunction()
        {
            // given
            var visitor = new SimplifyingFormulaVisitor();
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
        public void TestVisitAlternative()
        {
            // given
            var visitor = new SimplifyingFormulaVisitor();
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
        public void TestVisitEquivalence()
        {
            // given
            var visitor = new SimplifyingFormulaVisitor();
            var (left, leftResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var (right, rightResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var equivalence = new Equivalence(left, right);
            var expected = new Conjunction(
                new Alternative(new Negation(leftResult), rightResult),
                new Alternative(new Negation(rightResult), leftResult)
            );
            // when
            var result = visitor.Visit(equivalence);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitImplication()
        {
            // given
            var visitor = new SimplifyingFormulaVisitor();
            var (antecedent, antecedentResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var (consequent, consequentResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var implication = new Implication(antecedent, consequent);
            var expected = new Alternative(new Negation(antecedentResult), consequentResult);
            // when
            var result = visitor.Visit(implication);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestVisitConstant()
        {
            // given
            var constant = Constant.Falsity;
            var visitor = new SimplifyingFormulaVisitor();
            // when
            var result = visitor.Visit(constant);
            // then
            Assert.AreSame(constant, result);
        }

        [Test()]
        public void TestVisitLiteral()
        {
            // given
            var literal = new Literal(new Fluent("fluent"));
            var visitor = new SimplifyingFormulaVisitor();
            // when
            var result = visitor.Visit(literal);
            // then
            Assert.AreSame(literal, result);
        }

        [Test()]
        public void TestVisitNegation()
        {
            // given
            var visitor = new SimplifyingFormulaVisitor();
            var (formula, formulaResult) = VisitorTestUtils.MockFormulaAndAcceptResult(visitor);
            var negation = new Negation(formula);
            var expected = new Negation(formulaResult);
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }
    }
}