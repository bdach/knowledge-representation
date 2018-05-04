using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DynamicSystem.MinimizeNew
{
    /// <summary>
    /// Class for generating transition function
    /// </summary>
    class TransitionFunctionGenerator
    {
        public TransitionFunction GenerateTransitionFunction(Dictionary<(State, CompoundAction), HashSet<State>> res_0, Dictionary<(CompoundAction, State, State), HashSet<Fluent>> newSet)
        {
            //TODO: implement
            throw new NotImplementedException();
        }
    }
}
