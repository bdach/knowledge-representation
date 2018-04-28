using Model;
using Model.ActionLanguage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSystem.SetGeneration
{
    /// <inheritdoc />
    /// <summary>
    /// Class for generating subsets of given sets, satisfying certain conditions.
    /// </summary>
    public static class SetGenerator
    {
        /// <summary>
        /// Creates all possible subsets of given actions as collection of <see cref="CompoundAction"/>s.
        /// </summary>
        /// <param name="actions">Iterable collection of actions, representing set of actions, for which subsets are to be generated.</param>
        /// <returns>Iterable collection of <see cref="CompoundAction"/>s, where each <see cref="CompoundAction"/> represents one subset of actions.</returns>
        public static IEnumerable<CompoundAction> GetCompoundActions(IEnumerable<Model.Action> actions)
        {
            var allSubsets = SubSetsOf(actions);
            foreach (var actionSubset in allSubsets)
            {
                yield return new CompoundAction(actionSubset);
            }
        }

        /// <summary>
        /// Generates all states satisfying all initially conditions.
        /// </summary>
        /// <param name="fluents">Iterable collection of <see cref="Fluent"/>s available in the system.</param>
        /// <param name="actionDomain">Action domain of the system, containing defined action statements.</param>
        /// <returns>Iterable collection of <see cref="State"/>s satisfying initially conditions.</returns>
        public static IEnumerable<State> GetInitialStates(IEnumerable<Fluent> fluents, ActionDomain actionDomain)
        {
            return GetFilteredStatesSet(fluents, (state => actionDomain.InitialValueStatements.All(initialStatement => initialStatement.InitialCondition.Evaluate(state))));
        }

        /// <summary>
        /// Generates all states satisfying all always conditions.
        /// </summary>
        /// <param name="fluents">Iterable collection of <see cref="Fluent"/>s available in the system.</param>
        /// <param name="actionDomain">Action domain of the system, containing defined action statements.</param>
        /// <returns>Iterable collection of <see cref="State"/>s satisfying always conditions.</returns>
        public static IEnumerable<State> GetAdmissibleStates(IEnumerable<Fluent> fluents, ActionDomain actionDomain)
        {
            return GetFilteredStatesSet(fluents, (state => actionDomain.ConstraintStatements.All(contraintStatement => contraintStatement.Constraint.Evaluate(state))));
        }

        /// <summary>
        /// Generic function that generates all subsets of given iterable collection.
        /// </summary>
        /// <typeparam name="T">Type of elements in given collection.</typeparam>
        /// <param name="source">Iterable collection of elements for which sets are to be generated.</param>
        /// <returns>Iterable collection of subsets, where each subset is represented as another iterable collection.</returns>
        private static IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source)
        {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

            var element = source.Take(1);

            var setsNotContainingElement = SubSetsOf(source.Skip(1));
            var setsContainingElement = setsNotContainingElement.Select(set => element.Concat(set));

            return setsContainingElement.Concat(setsNotContainingElement);
        }

        /// <summary>
        /// Helper function to generate all possible states from given set of fluents.
        /// </summary>
        /// <param name="fluents">Fluents available in the decision system.</param>
        /// <returns>Iterable collection of all possible states.</returns>
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

        /// <summary>
        /// Helper function to generate and filter all possible states in the decision system.
        /// </summary>
        /// <param name="fluents">Fluents available in the decision system.</param>
        /// <param name="predicate">Condition that has to be satisfied by a <see cref="State"/>.</param>
        /// <returns>Iterable collection of states satisfying given condition.</returns>
        private static IEnumerable<State> GetFilteredStatesSet(IEnumerable<Fluent> fluents, Func<State, bool> predicate)
        {
            var allStates = CreateSetsFromFluents(fluents);
            return allStates.Where(predicate);
        }
    }
}
