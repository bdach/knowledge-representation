using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    /// <inheritdoc />
    /// <summary>
    /// Currently the only implementation of the <see cref="IDecompositionGenerator"/> interface.
    /// </summary>
    public class DecompositionGenerator : IDecompositionGenerator
    {
        /// <inheritdoc />
        public IEnumerable<HashSet<Action>> GetDecompositions(ActionDomain domain, HashSet<Action> actions, State state)
        {
            var result = new List<HashSet<Action>>();
            var listQueue = new Queue<HashSet<Action>>();
            listQueue.Enqueue(actions);
            //BFS
            while (listQueue.Count > 0)
            {
                var list = listQueue.Dequeue();
                //New solution is not maximal, it is a subset of an existing solution
                if(result.Any(r => r.IsSupersetOf(list)))
                    continue;
                //Add lists of actions wihtout confilcting pairs of actions
                if (!list.AllPairs().Any(pair => pair.a.IsConflicting(pair.b, state, domain)))
                    result.Add(list);
                //Generate new lists with one less element
                foreach (var action in list)
                {
                    listQueue.Enqueue(new HashSet<Action>(list.Where(l => !l.Equals(action))));
                }
            }
            return result;
        }
    }
}