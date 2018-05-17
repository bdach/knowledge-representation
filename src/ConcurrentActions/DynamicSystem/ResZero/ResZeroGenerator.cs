﻿using System.Collections.Generic;
using DynamicSystem.Decomposition;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.ResZero
{
    public static class ResZeroGenerator
    {
        /// <summary>
        /// Generates the Res_0 mapping for a given action domain.
        /// </summary>
        /// <param name="actionDomain">The <see cref="ActionDomain"/> instance describing the action domain.</param>
        /// <param name="compoundActions">The set of <see cref="CompoundAction"/> instances to calculate the mapping values for.</param>
        /// <param name="states">The set of <see cref="State"/> instances to calculate the mapping values for.</param>
        /// <returns></returns>
        public static TransitionFunction GenerateResZero(ActionDomain actionDomain, HashSet<CompoundAction> compoundActions, HashSet<State> states)
        {
            var transitionFunction = new TransitionFunction(compoundActions, states);
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
                    transitionFunction[compoundAction, state] = resultStates;
                }
            }
            return transitionFunction;
        }
    }
}