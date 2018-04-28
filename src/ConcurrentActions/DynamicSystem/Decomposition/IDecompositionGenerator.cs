using System.Collections.Generic;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    /// <summary>
    /// Interface for classes that can return decompositions for a given set of actions.
    /// </summary>
    public interface IDecompositionGenerator
    {
        /// <summary>
        /// Return the decomposition of the given set of actions.
        /// </summary>
        /// <param name="domain">The action domain where the actions are defined.</param>
        /// <param name="actions">Set of unique actions. Every element of the set should be a single action, not a compound one.</param>
        /// <param name="state">State where the actions are meant to be executed.</param>
        /// <returns>The actions decomposed into non-confilcting subsets which are maximal wrt. inclusion.</returns>
        IEnumerable<HashSet<Action>> GetDecompositions(ActionDomain domain, HashSet<Action> actions, State state);
    }
}
