using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.Decomposition
{
    public interface IDecompositionGenerator
    {
        IEnumerable<List<Model.Action>> GetDecompositions(ActionDomain domain, CompoundAction compoundAction, State state);
    }
}
