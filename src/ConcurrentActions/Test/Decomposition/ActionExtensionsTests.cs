using System.Collections.Generic;
using DynamicSystem.Decomposition;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;

namespace Test.Decomposition
{
    [TestFixture]
    public class ActionExtensionsTests
    {
        [Test]
        public void GivenActionDomainWithInconsistentCausesStatements_IsConfilctingForTheActions_ReturnsTrue()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), DecompositionTestUtils.CreateBinaryConjunction("q", "r", true)));
            var state = new State(new List<Fluent>{new Fluent("q"), new Fluent("r") }, new List<bool> {true, true});
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(true);
        }

        public void GivenActionDomainWithInconsistentCausesStatements_IsConfilctingForOtherActions_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), DecompositionTestUtils.CreateBinaryConjunction("q", "r", true)));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("C"), new Literal(new Fluent("q"))));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("C"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithInconsistentCausesStatements_IsConfilctingForTheActionsWithPreconditionFalse_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), new Literal(new Fluent("q"), true), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), DecompositionTestUtils.CreateBinaryConjunction("q", "r", true)));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithReleasesAndCausesStatementsWithTheSameFluent_IsConfilctingForTheActions_ReturnsTrue()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r") ));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(true);
        }

        [Test]
        public void GivenActionDomainWithReleasesAndCausesStatementsWithTheSameFluent_IsConfilctingForOtherActions_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("C"), new Fluent("s")));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r"), new Fluent("s") }, new List<bool> { true, true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("C"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithReleasesAndCausesStatementsWithTheSameFluen_IsConfilctingForTheActionsWithPreconditionFalse_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), DecompositionTestUtils.CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"),  new Fluent("r"), new Literal(new Fluent("q"), true)));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithReleasesStatementsForTheSameFluent_IsConfilctingForTheActions_ReturnsTrue()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("A"), new Fluent("r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r")));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(true);
        }

        [Test]
        public void GivenActionDomainWithReleasesStatementsForTwoDifferentFluent_IsConfilctingForTheActions_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("A"), new Fluent("r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("q")));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }
        [Test]
        public void GivenActionDomainWithReleasesStatementsForTheSameFluent_IsConfilctingForTheActionsWithPreconditionFalse_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("A"), new Fluent("r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r"), new Literal(new Fluent("q"), true)));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }
    }
}
