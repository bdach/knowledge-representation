using NUnit.Framework;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using DynamicSystem.SetGeneration;
using System.Linq;
using Model.Forms;

namespace Test.SetGeneration
{
    [TestFixture]
    public class AdmissibleStatesTest
    {
        private Fluent[] fluents = new Fluent[] { new Fluent("firstFluent"), new Fluent("secondFluent"), new Fluent("thirdFluent") };

        [Test]
        public void TestNoConstraintStatement()
        {
            //given
            var actionDomain = new ActionDomain();
            //when
            var admissibleStates = SetGenerator.GetAdmissibleStates(fluents, actionDomain);
            //then
            admissibleStates.Count().Should().Be((int)System.Math.Pow(2, fluents.Count()));
        }

        [Test]
        public void TestSingleConstraintStatement()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Literal(fluents[0])));
            //when
            var admissibleStates = SetGenerator.GetAdmissibleStates(fluents, actionDomain);
            //then
            admissibleStates.All(state => state[fluents[0]]).Should().BeTrue();
        }

        [Test]
        public void TestManyConstraintStatements()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Literal(fluents[0])));
            actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Literal(fluents[1])));
            //when
            var admissibleStates = SetGenerator.GetAdmissibleStates(fluents, actionDomain);
            //then
            admissibleStates.All(state => state[fluents[0]] && state[fluents[1]]).Should().BeTrue();
        }

        [Test]
        public void TestNoAdmissibleStates()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.ConstraintStatements.Add(new ConstraintStatement(new Conjunction(new Literal(fluents[0]), new Literal(fluents[0], true))));
            //when
            var admissibleStates = SetGenerator.GetAdmissibleStates(fluents, actionDomain);
            //then
            admissibleStates.Count().Should().Be(0);
        }
    }
}
