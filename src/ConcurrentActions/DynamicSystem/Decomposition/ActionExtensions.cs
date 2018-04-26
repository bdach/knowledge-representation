using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using DynamicSystem.DNF;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    public static class ActionExtensions
    {
        public static bool IsConflicting(this Action action, Action other, State state, ActionDomain actionDomain)
        {
            //Case 1: Two conflicting causes statements
            var actionCausesStatements = actionDomain.EffectStatements
                .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(state)).ToList();
            var otherCausesStatements = actionDomain.EffectStatements
                .Where(e => e.Action.Equals(other) && e.Precondition.Evaluate(state)).ToList();
            //For every causes sentence pair check if there is a conflict in the postcondition
            if (actionCausesStatements.CartesianProduct(otherCausesStatements).Any(pair =>
            {
                var aDnf = pair.a.Postcondition.ToDnf();
                var bDnf = pair.b.Postcondition.ToDnf();
                return aDnf.Conflicts(bDnf);
            })) return true;

            //Case 2: Causes and releases statement
            //Case 2a: Causes with action and releases with other
            var releasesOtherStatements = actionDomain.FluentReleaseStatements
                .Where(e => e.Action.Equals(other) && e.Precondition.Evaluate(state)).ToList();
            if (actionCausesStatements.CartesianProduct(releasesOtherStatements).Any(pair =>
            {
                var aDnf = pair.a.Postcondition.ToDnf();
                return aDnf.Conjunctions.Any(c => c.Literals.Any(l => l.Fluent.Equals(pair.b.Fluent)));
            })) return true;
            //Case 2b: Causes with other and releases with action
            var releasesActionStatements = actionDomain.FluentReleaseStatements
                .Where(e => e.Action.Equals(action) && e.Precondition.Evaluate(state)).ToList();
            if (otherCausesStatements.CartesianProduct(releasesActionStatements).Any(pair =>
            {
                var oDnf = pair.a.Postcondition.ToDnf();
                return oDnf.Conjunctions.Any(c => c.Literals.Any(l => l.Fluent.Equals(pair.b.Fluent)));
            })) return true;

            //Case 3: No confilcts!
            return false;
        }

    }
}