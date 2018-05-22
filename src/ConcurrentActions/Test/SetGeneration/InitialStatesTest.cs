using Model;
using NUnit.Framework;
using FluentAssertions;
using System.Collections.Generic;
using Model.ActionLanguage;
using DynamicSystem.SetGeneration;
using System.Linq;
using Model.Forms;

namespace Test.SetGeneration
{
    [TestFixture]
    public class InitialStatesTest
    {
        private Fluent[] fluents = new Fluent[] { new Fluent("firstFluent"), new Fluent("secondFluent"), new Fluent("thirdFluent") };

        [Test]
        public void TestZeroInitialStatements()
        {
            //given
            var actionDomain = new ActionDomain();
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.Count().Should().Be((int)System.Math.Pow(2, fluents.Count()));
        }

        [Test]
        public void TestSingleInitialStatement()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(fluents[0])));
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.All(state => state[fluents[0]]).Should().BeTrue();
        }

        [Test]
        public void TestManyInitialStatements()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(fluents[0])));
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(fluents[1])));
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Literal(fluents[2])));
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.All(state => state[fluents[0]] && state[fluents[1]] && state[fluents[2]]).Should().BeTrue();
        }

        [Test]
        public void TestManyInitialStatementsAsConjunction()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Conjunction(new Conjunction(new Literal(fluents[0]),
                                                    new Literal(fluents[1])), new Literal(fluents[2]))));
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.All(state => state[fluents[0]] && state[fluents[1]] && state[fluents[2]]).Should().BeTrue();
        }

        [Test]
        public void TestNoInitialStates()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Conjunction(new Literal(fluents[0]), new Literal(fluents[0], true))));
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.Count().Should().Be(0);
        }

        [Test]
        public void TestIntegrityConditions()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.InitialValueStatements.Add(new InitialValueStatement(new Conjunction(new Literal(fluents[1]), new Literal(fluents[2]))));
            actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Equivalence(new Literal(fluents[0]),
                                                  new Equivalence(new Literal(fluents[1]), new Literal(fluents[2])))));
            //when
            var initialStates = SetGenerator.GetInitialStates(fluents, actionDomain);
            //then
            initialStates.Count().Should().Be(1);
            foreach (var state in initialStates)
            {
                (state[fluents[0]] == (state[fluents[1]] == state[fluents[2]])).Should().BeTrue();
            }
        }
    }
}
