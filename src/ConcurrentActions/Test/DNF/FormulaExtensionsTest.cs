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
        [Test]
        public void ShouldConvertFormulaToDnf()
        {
            // example from https://math.stackexchange.com/questions/228969/how-to-convert-formula-to-disjunctive-normal-form
            var statement = new Conjunction(
                new Implication(
                    new Conjunction(
                        new Literal(new Fluent("P")),
                        new Literal(new Fluent("Q"))
                    ),
                    new Literal(new Fluent("R"))
                ),
                new Implication(
                    new Negation(new Conjunction(
                        new Literal(new Fluent("P")),
                        new Literal(new Fluent("Q"))
                    )),
                    new Literal(new Fluent("R"))
                )
            );

            var dnf = statement.ToDnf();
            Assert.IsTrue(dnf.Conjunctions.Any(a => dnf.Conjunctions.Any(a.Conflicts)));
        }
    }
}