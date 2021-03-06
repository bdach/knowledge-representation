﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF;
using DynamicSystem.DNF.Visitors;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;
using Action = System.Action;

namespace Test.DNF.Visitors
{
    [TestFixture]
    public class NaryConjunctionGeneratingFormulaVisitorTest
    {
        [Test()]
        public void TestGeneration()
        {
            // given
            var a = new Literal(new Fluent("a"));
            var b = new Literal(new Fluent("b"));
            var c = new Literal(new Fluent("c"));
            var formula = new Alternative(
                new Conjunction(a, b),
                new Conjunction(c, Constant.Truth)
            );
            var generated = new List<NaryConjunction>();
            var visitor = new NaryConjunctionGeneratingFormulaVisitor(generated.Add);
            // when
            formula.Accept(visitor);
            // then
            CollectionAssert.AreEquivalent(new List<NaryConjunction>()
            {
                new NaryConjunction(
                    new List<Literal>() {a, b}, new List<Constant>()
                ),
                new NaryConjunction(
                    new List<Literal>() {c}, new List<Constant>() {Constant.Truth}
                )
            }, generated);
        }
    }
}