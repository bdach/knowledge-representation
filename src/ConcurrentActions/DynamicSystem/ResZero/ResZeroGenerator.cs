using System.Collections.Generic;
using DynamicSystem.Decomposition;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.ResZero
{
    public static class ResZeroGenerator
    {
        // TODO: change this back to TransitionFunction
        public static Dictionary<(CompoundAction, State), HashSet<State>> GenerateResZero(ActionDomain actionDomain, HashSet<CompoundAction> compoundActions, HashSet<State> states)
        {
            var transitionFunction = new Dictionary<(CompoundAction, State), HashSet<State>>();
            var resZero = new ResZero(actionDomain.EffectStatements, states);
            var decompositionGenerator = new DecompositionGenerator();

            foreach (var compoundAction in compoundActions)
            {
                foreach (var state in states)
                {
                    var decompositions = decompositionGenerator.GetDecompositions(actionDomain, compoundAction.Actions, state);
                    var resultStates = new HashSet<State>();
                    foreach (var decomposition in decompositions)
                    {
                        var decompositionResult = resZero.GetStates(state, decomposition);
                        resultStates.UnionWith(decompositionResult);
                    }
                    transitionFunction[(compoundAction, state)] = resultStates;
                }
            }
            return transitionFunction;
        }
    }
}