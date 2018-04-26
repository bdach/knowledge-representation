using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    class DecompositionGenerator : IDecompositionGenerator
    {
        public IEnumerable<List<Action>> GetDecompositions(ActionDomain domain, CompoundAction compoundAction, State state)
        {
            var result = new List<List<Action>>();
            var listQueue = new Queue<List<Action>>();
            listQueue.Enqueue(compoundAction.Actions.ToList());
            //BFS
            while (listQueue.Count > 0)
            {
                var list = listQueue.Dequeue();
                //New solution is not maximal, it is a subset of an existing solution
                if(result.Any(r => list.All(r.Contains)))
                    continue;
                //Add lists of actions wihtout confilcting pairs of actions
                if (!list.AllPairs().Any(pair => pair.a.IsConflicting(pair.b, state, domain)))
                    result.Add(list);
                //Generate new lists with one less element
                //TODO: Maybe don't generate duplicates?
                foreach (var action in list)
                {
                    listQueue.Enqueue(list.Where(l => !l.Equals(action)).ToList());
                }
            }
            return result;
        }
    }
}