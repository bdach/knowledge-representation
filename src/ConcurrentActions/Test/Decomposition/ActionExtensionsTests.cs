using System.Collections.Generic;
using DynamicSystem.Decomposition;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;
using System.Linq;

namespace Test.Decomposition
{
    [TestFixture]
    public class ActionExtensionsTests
    {
        [Test]
        public void GivenActionDomainWithConfilctingCausesStatements_IsConfilctingForTheActions_ReturnsTrue()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), CreateBinaryConjunction("q", "r", true)));
            var state = new State(new List<Fluent>{new Fluent("q"), new Fluent("r") }, new List<bool> {true, true});
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(true);
        }

        public void GivenActionDomainWithConfilctingCausesStatements_IsConfilctingForOtherActions_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), CreateBinaryConjunction("q", "r", true)));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("C"), new Literal(new Fluent("q"))));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("C"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithConfilctingCausesStatements_IsConfilctingForTheActionsWithPreconditionFalse_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), new Literal(new Fluent("q"), true), CreateBinaryConjunction("q", "r")));
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("B"), CreateBinaryConjunction("q", "r", true)));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithConfilctingReleasesStatements_IsConfilctingForTheActions_ReturnsTrue()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r") ));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(true);
        }

        [Test]
        public void GivenActionDomainWithConfilctingReleasesStatements_IsConfilctingForOtherActions_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"), new Fluent("r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("C"), new Fluent("s")));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r"), new Fluent("s") }, new List<bool> { true, true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("C"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        [Test]
        public void GivenActionDomainWithConfilctingReleasesStatements_IsConfilctingForTheActionsWithPreconditionFalse_ReturnsFalse()
        {
            //given
            var actionDomain = new ActionDomain();
            actionDomain.EffectStatements.Add(new EffectStatement(new Action("A"), CreateBinaryConjunction("q", "r")));
            actionDomain.FluentReleaseStatements.Add(new FluentReleaseStatement(new Action("B"),  new Fluent("r"), new Literal(new Fluent("q"), true)));
            var state = new State(new List<Fluent> { new Fluent("q"), new Fluent("r") }, new List<bool> { true, true });
            //when
            var conflicting = new Action("A").IsConflicting(new Action("B"), state, actionDomain);
            //then
            conflicting.Should().Be(false);
        }

        private static IFormula CreateBinaryConjunction(string nameA, string nameB, bool negatedA = false, bool negatedB = false)
        {
            return new Conjunction(new Literal(new Fluent(nameA), negatedA), new Literal(new Fluent(nameB), negatedB));
        }
    }
}
