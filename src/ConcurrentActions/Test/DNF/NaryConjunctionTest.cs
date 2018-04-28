using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF;
using FluentAssertions;
using Model;
using Model.Forms;
using Moq;
using NUnit.Framework;

namespace Test.DNF
{
    [TestFixture]
    public class NaryConjunctionTest
    {
        [TestCaseSource(typeof(NaryConjunctionTestCaseSource), "Cases")]
        public void TestConflicts(NaryConjunction a, NaryConjunction b, bool expected)
        {
            Assert.AreEqual(a.Conflicts(b), expected);
        }

        public static class NaryConjunctionTestCaseSource
        {
            private static readonly object[] Cases =
            {
                new object[]
                {
                    new NaryConjunction(new List<Literal>() { }, new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() { }, new List<Constant>() { }),
                    false
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() { }, new List<Constant>() {Constant.Falsity}),
                    new NaryConjunction(new List<Literal>() { }, new List<Constant>() { }),
                    true
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), false)},
                        new List<Constant>() { }),
                    true
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentB"), false)},
                        new List<Constant>() { }),
                    false
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    false
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() { },
                        new List<Constant>() { }),
                    false
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() { },
                        new List<Constant>() {Constant.Truth}),
                    false
                },
                new object[]
                {
                    new NaryConjunction(new List<Literal>() {new Literal(new Fluent("fluentA"), true)},
                        new List<Constant>() { }),
                    new NaryConjunction(new List<Literal>() { },
                        new List<Constant>() {Constant.Falsity}),
                    true
                }
            };
        }
    }
}