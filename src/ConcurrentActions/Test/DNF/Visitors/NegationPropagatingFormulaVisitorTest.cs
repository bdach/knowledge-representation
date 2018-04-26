using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF.Visitors;
using Model;
using Model.Forms;
using NUnit.Framework;

namespace Test.DNF.Visitors
{
    [TestFixture]
    public class NegationPropagatingFormulaVisitorTest
    {
        [Test()]
        public void TestNegateConjunction()
        {
            // given
            var negation = new Negation(new Conjunction(Constant.Truth, Constant.Falsity));
            var visitor = new NegationPropagatingFormulaVisitor();
            var expected = new Alternative(Constant.Falsity, Constant.Truth);
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestNegateAlternative()
        {
            // given
            var negation = new Negation(new Alternative(Constant.Truth, Constant.Falsity));
            var visitor = new NegationPropagatingFormulaVisitor();
            var expected = new Conjunction(Constant.Falsity, Constant.Truth);
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestNegateEquivalence()
        {
            // given
            var negation = new Negation(new Equivalence(Constant.Truth, Constant.Falsity));
            var visitor = new NegationPropagatingFormulaVisitor();
            var expected = new Alternative(
                new Conjunction(Constant.Truth, Constant.Truth),
                new Conjunction(Constant.Falsity, Constant.Falsity)
            );
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestNegateImplication()
        {
            // given
            var negation = new Negation(new Implication(Constant.Truth, Constant.Falsity));
            var visitor = new NegationPropagatingFormulaVisitor();
            var expected = new Conjunction(Constant.Truth, Constant.Truth);
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestNegateNegation()
        {
            // given
            var visitor = new NegationPropagatingFormulaVisitor();
            var negation = new Negation(new Negation(Constant.Truth));
            var expected = Constant.Truth;
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }

        [Test()]
        public void TestNegateTruthAndFalsity()
        {
            // given
            var visitor = new NegationPropagatingFormulaVisitor();
            var negationOfTruth = new Negation(Constant.Truth);
            var negationOfFalsity = new Negation(Constant.Falsity);
            // expect
            Assert.AreEqual(Constant.Falsity, visitor.Visit(negationOfTruth));
            Assert.AreEqual(Constant.Truth, visitor.Visit(negationOfFalsity));
        }

        [Test()]
        public void TestNegateLiteral()
        {
            // given
            var visitor = new NegationPropagatingFormulaVisitor();
            var fluent = new Fluent("fluent");
            var negation = new Negation(new Literal(fluent));
            var expected = new Literal(fluent, true);
            // when
            var result = visitor.Visit(negation);
            // then
            Assert.AreEqual(expected, result);
        }
    }
}