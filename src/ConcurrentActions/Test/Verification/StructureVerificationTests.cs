using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.Verification;
using FluentAssertions;
using Model;
using Model.ActionLanguage;
using Model.Forms;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Action = Model.Action;

namespace Test.Verification
{
    [TestFixture]
    public class StructureVerificationTests
    {
        private readonly Structure _structure;

        public StructureVerificationTests()
        {
            var fluents = new[] { new Fluent("test1"), new Fluent("test2") };

            var state0 = new State(fluents, new[] { true, true });
            var state1 = new State(fluents, new[] { true, true });
            var state2 = new State(fluents, new[] { true, false });

            var states = new[] { state0, state1, state2 };
            var actions = new[] { new CompoundAction(new Action("action1"))};

            var transitionFunction =
                new TransitionFunction(actions, states)
                {
                    [actions[0], state0] = new HashSet<State> { state1, state2},
                };

            _structure = new Structure(states, state0, transitionFunction);
        }

        [Test]
        public void CheckStatements_GivenCorrectValueStatement_ShouldReturnTrue()
        {
            //arrange
            var condition = new Literal(new Fluent("test1"), false);
            var action = new Action("action1");
            var valueStatement = new ValueStatement(condition, action);
            var domain = new ActionDomain()
            {
                ValueStatements = {valueStatement}
            };

            //act
            var res = StructureVerification.CheckStatements(domain, _structure);

            //assert
            res.Should().BeTrue();
        }

        [Test]
        public void CheckStatements_GivenCorrectObservationStatement_ShouldReturnTrue()
        {
            //arrange
            var condition = new Literal(new Fluent("test2"), false);
            var action = new Action("action1");
            var observationStatement = new ObservationStatement(condition, action);
            var domain = new ActionDomain()
            {
                ObservationStatements = { observationStatement }
            };

            //act
            var res = StructureVerification.CheckStatements(domain, _structure);

            //assert
            res.Should().BeTrue();
        }

        [Test]
        public void CheckStatements_GivenWrongValueStatement_ShouldReturnFalse()
        {
            //arrange
            var condition = new Literal(new Fluent("test2"), false);
            var action = new Action("action1");
            var valueStatement = new ValueStatement(condition, action);
            var domain = new ActionDomain()
            {
                ValueStatements = { valueStatement }
            };

            //act
            var res = StructureVerification.CheckStatements(domain, _structure);

            //assert
            res.Should().BeFalse();
        }
    }
}
