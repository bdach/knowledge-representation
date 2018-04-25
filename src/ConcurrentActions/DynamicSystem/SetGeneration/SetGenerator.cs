using Model;
using Model.ActionLanguage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSystem.SetGeneration
{
    public class SetGenerator
    {
        public static IEnumerable<CompoundAction> GetCompoundActions(IEnumerable<Model.Action> actions)
        {
            var allSubsets = SubSetsOf(actions);
            foreach (var actionSubset in allSubsets)
            {
                if (actionSubset.Any())
                {
                    yield return new CompoundAction(actionSubset);
                }
            }
        }

        public static IEnumerable<State> GetInitialStates(IEnumerable<Fluent> fluents, ActionDomain actionDomain)
        {
            return GetFilteredStatesSet(fluents, (state => actionDomain.InitialValueStatements.All(initialStatement => initialStatement.InitialCondition.Evaluate(state))));
        }

        public static IEnumerable<State> GetAdmissibleStates(IEnumerable<Fluent> fluents, ActionDomain actionDomain)
        {
            return GetFilteredStatesSet(fluents, (state => actionDomain.ConstraintStatements.All(contraintStatement => contraintStatement.Constraint.Evaluate(state))));
        }

        private static IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source)
        {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

            var element = source.Take(1);

            var setsNotContainingElement = SubSetsOf(source.Skip(1));
            var setsContainingElement = setsNotContainingElement.Select(set => element.Concat(set));

            return setsContainingElement.Concat(setsNotContainingElement);
        }

        private static IEnumerable<State> CreateSetsFromFluents(IEnumerable<Fluent> fluents)
        {
            var fluentSubsets = SubSetsOf(fluents);
            foreach (var fluentSubset in fluentSubsets)
            {
                var fluentState = new Dictionary<Fluent, bool>(fluents.Count());
                foreach (var fluent in fluents)
                {
                    fluentState.Add(fluent, false);
                }

                foreach (var fluent in fluentSubset)
                {
                    fluentState[fluent] = true;
                }

                yield return new State(fluentState);
            }
        }

        private static IEnumerable<State> GetFilteredStatesSet(IEnumerable<Fluent> fluents, Func<State, bool> predicate)
        {
            var allStates = CreateSetsFromFluents(fluents);
            return allStates.Where(predicate);
        }
    }
}
