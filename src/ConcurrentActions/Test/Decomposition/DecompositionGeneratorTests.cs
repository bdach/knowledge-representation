using System.Collections.Generic;
using System.Linq;
using DynamicSystem.Decomposition;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;


namespace Test.Decomposition
{
    [TestFixture]
    public class DecompositionGeneratorTests
    {
        [Test]
        public void GivenACompoundActionWithoutConfilcts_DecompositionGenerator_ReturnsOriginalActionSet()
        {
            //given
            var generator = new DecompositionGenerator();
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), new Literal(new Fluent("q"))));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("C"), new Literal(new Fluent("q"))));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            var actions = new HashSet<Action> {new Action("A"), new Action("B"), new Action("C")};
            //when
            var decomposition = generator.GetDecompositions(actionDomain, actions, state).ToList();
            //then
            decomposition.Count.Should().Be(1);
            decomposition.First().Should().Contain(actions);
        }
        [Test]
        public void GivenACompoundActionWithConfilcts_DecompositionGenerator_ReturnsProperDecomposition()
        {
            //given
            var generator = new DecompositionGenerator();
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), DecompositionTestUtils.CreateBinaryConjunction("q", "r", false, true)));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("C"), new Literal(new Fluent("q"))));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            var actions = new HashSet<Action> { new Action("A"), new Action("B"), new Action("C") };
            //when
            var decomposition = generator.GetDecompositions(actionDomain, actions, state).ToList();
            //then
            decomposition.Count.Should().Be(2);
            decomposition.Should().Contain(e => e.Count == 2 && e.Contains(new Action("A")) && e.Contains(new Action("C")));
            decomposition.Should().Contain(e => e.Count == 2 && e.Contains(new Action("B")) && e.Contains(new Action("B")));
        }
    }
}
