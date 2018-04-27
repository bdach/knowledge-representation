using System.Collections.Generic;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    public interface IDecompositionGenerator
    {
        IEnumerable<HashSet<Action>> GetDecompositions(ActionDomain domain, HashSet<Action> actions, State state);
    }
}
