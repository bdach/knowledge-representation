using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF;
using Model;
using Model.Forms;
using NUnit.Framework;

namespace Test.DNF
{
    [TestFixture]
    public class FormulaExtensionsTest
    {
        [Ignore("equals bug?")]
        public void TestToDnf()
        {
            // given
            var statement = new Implication(
                new Conjunction(
                    new Literal(new Fluent("P")),
                    new Literal(new Fluent("Q"))
                ),
                new Literal(new Fluent("R"))
            );
            var expectedFormula = new Alternative(
                new Alternative(
                    new Literal(new Fluent("P"), true),
                    new Literal(new Fluent("Q"), true)
                ),
                new Literal(new Fluent("R"))
            );
            var expectedConjunctions = new List<NaryConjunction>()
            {
                new NaryConjunction(new List<Literal>() {new Literal(new Fluent("P"), true)}, new List<Constant>()),
                new NaryConjunction(new List<Literal>() {new Literal(new Fluent("Q"), true)}, new List<Constant>()),
                new NaryConjunction(new List<Literal>() {new Literal(new Fluent("R"))}, new List<Constant>()),
            };
            // when
            var dnf = statement.ToDnf();
            // then
            Assert.AreEqual(new DnfFormula(expectedFormula, expectedConjunctions), dnf);
        }
    }
}