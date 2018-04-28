using NUnit.Framework;
using FluentAssertions;
using Model;
using DynamicSystem.SetGeneration;
using System.Linq;
using System.Collections.Generic;

namespace Test.SetGeneration
{
    [TestFixture]
    public class CompoundActionTest
    {
        [Test]
        public void TestSingleActionSubsets()
        {
            //given
            var actions = new Action[] { new Action("singleAction") };
            //when
            var actionsSets = SetGenerator.GetCompoundActions(actions);
            //then
            actionsSets.Count().Should().Be(2);
            actionsSets.Any(actionsSet => actionsSet.Actions.Contains(actions[0])).Should().BeTrue();
            actionsSets.Any(actionsSet => !actionsSet.Actions.Any()).Should().BeTrue();
        }

        [Test]
        public void TestManyActionsSubsets()
        {
            //given
            var actions = new Action[] { new Action("firstAction"), new Action("secondAction"), new Action("thirdAction") };
            var actualSubsets = new List<Action[]>()
            {
                new Action[] { },
                new Action[] { actions[0] },
                new Action[] { actions[1] },
                new Action[] { actions[2] },
                new Action[] { actions[0], actions[1]},
                new Action[] { actions[0], actions[2]},
                new Action[] { actions[1], actions[2]},
                new Action[] { actions[0], actions[1], actions[2] }
            };
            //when
            var actionsSets = SetGenerator.GetCompoundActions(actions);
            //then
            actionsSets.Count().Should().Be((int)System.Math.Pow(2, actions.Count()));
            foreach (var actualSubset in actualSubsets)
            {
                actionsSets.Any(actionsSet =>
                {
                    if (actionsSet.Actions.Count == actualSubset.Count())
                    {
                        foreach (var action in actualSubset)
                        {
                            if (!actionsSet.Actions.Contains(action))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                    return false;
                }).Should().BeTrue();
            }
        }
    }
}
