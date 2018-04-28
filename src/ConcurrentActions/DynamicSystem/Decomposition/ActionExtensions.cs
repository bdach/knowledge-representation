using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using DynamicSystem.DNF;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    /// <summary>
    /// Static class for extension functions of the <see cref="Action"/> class
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Returns wheter the given actions conflicts with another one in the given state and action domain.
        /// </summary>
        /// <param name="action">First action to check</param>
        /// <param name="other">Second action to check</param>
        /// <param name="state">State in which the actions are meant to be executed</param>
        /// <param name="actionDomain">Action domian where the actions are defined</param>
        /// <returns>True if the actions are conflicting, false otherwise</returns>
        public static bool IsConflicting(this Action action, Action other, State state, ActionDomain actionDomain)
        {
            //Case 1: Two conflicting causes statements
            var actionCausesStatements = GetEffectStatementsForAction(action, actionDomain, state);
            var otherCausesStatements = GetEffectStatementsForAction(other, actionDomain, state);
            //For every causes sentence pair check if there is a conflict in the postcondition
            if (actionCausesStatements.CartesianProduct(otherCausesStatements).Any(pair =>
            {
                var aDnf = pair.a.Postcondition.ToDnf();
                var bDnf = pair.b.Postcondition.ToDnf();
                return aDnf.Conflicts(bDnf);
            })) return true;

            //Case 2: Causes and releases statement
            if (CausesReleasesConflict(action, other, actionDomain, state) ||
                CausesReleasesConflict(other, action, actionDomain, state))
                return true;
            //Case 3: Conflicting Releases statements
            var actionReleasesStatementsFluents = GetReleasesStatementsForAction(action, actionDomain, state).Select(a => a.Fluent);
            var otherReleasesStatementsFluents = GetReleasesStatementsForAction(other, actionDomain, state).Select(o => o.Fluent);
            return actionReleasesStatementsFluents.Intersect(otherReleasesStatementsFluents).Any();
        }

        /// <summary>
        /// Checks if effect statements for action a have a fluent in common with action b releases statments
        /// </summary>
        /// <param name="a">Action for which the effect statements are checked</param>
        /// <param name="b">Action for which the releases statements are checked</param>
        /// <param name="actionDomain">Action domain where the actions and statments are defined</param>
        /// <param name="state">State where the actions are meant to be executed</param>
        /// <returns>True if they have fluents in common, false otherwise</returns>
        private static bool CausesReleasesConflict(Action a, Action b, ActionDomain actionDomain, IState state)
        {
            var actionACausesStatements = GetEffectStatementsForAction(a, actionDomain, state);
            var releasesBStatements = GetReleasesStatementsForAction(b, actionDomain, state);
            return actionACausesStatements.CartesianProduct(releasesBStatements).Any(pair =>
            {
                var aDnf = pair.a.Postcondition.ToDnf();
                return aDnf.Conjunctions.Any(c => c.Literals.Any(l => l.Fluent.Equals(pair.b.Fluent)));
            });
        }

        /// <summary>
        /// Returns all effect statements for the given action for which the precondition holds in the given state
        /// </summary>
        /// <param name="action">The action of the effect statements</param>
        /// <param name="actionDomain">Action domain where the actions and statments are defined</param>
        /// <param name="state">State where the actions are meant to be executed</param>
        /// <returns>List of the effect statements</returns>
        private static IList<EffectStatement> GetEffectStatementsForAction(Action action, ActionDomain actionDomain,
            IState state)
        {
            return actionDomain.EffectStatements
                .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(state)).ToList();
        }

        /// <summary>
        /// Returns all fluent release statements for the given action for which the precondition holds in the given state
        /// </summary>
        /// <param name="action">The action of the releases statements</param>
        /// <param name="actionDomain">Action domain where the actions and statments are defined</param>
        /// <param name="state">State where the actions are meant to be executed</param>
        /// <returns>List of the fluent release statements</returns>
        private static IList<FluentReleaseStatement> GetReleasesStatementsForAction(Action action, ActionDomain actionDomain,
            IState state)
        {
            return actionDomain.FluentReleaseStatements
                .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(state)).ToList();
        }

    }
}