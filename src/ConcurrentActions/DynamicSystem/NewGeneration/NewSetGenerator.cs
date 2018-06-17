﻿using System;
using System.Collections.Generic;
using Model;
using Model.ActionLanguage;
using Model.Forms;

namespace DynamicSystem.NewGeneration
{
    public static class NewSetGenerator
    {
        /// <summary>
        /// Calculates the New sets for each compound action, initial and resulting state.
        /// </summary>
        /// <param name="domain"><see cref="ActionDomain"/> instance to use.</param>
        /// <param name="signature">The <see cref="Signature"/> of the action language.</param>
        /// <param name="resZero"><see cref="TransitionFunction"/> instance representing the Res_0 mapping.</param>
        /// <returns><see cref="NewSetMapping"/> instance containing the New sets.</returns>
        [Obsolete]
        public static NewSetMapping GetNewSets(ActionDomain domain, Signature signature, TransitionFunction resZero)
        {
            var newMapping = new NewSetMapping();
            var newHelper = new NewSetHelper(domain, signature.Fluents);
            foreach (var pair in resZero.Arguments)
            {
                var resultStates = resZero[pair.action, pair.state];
                foreach (var resultState in resultStates)
                {
                    var newValue = newHelper.GetLiterals(pair.action.Actions, pair.state, resultState);
                    newMapping[pair.action, pair.state, resultState] = new HashSet<Literal>(newValue);
                }
            }
            return newMapping;
        }

        public static NewSetMapping GetNewSets(ActionDomain domain, Signature signature, TransitionFunction resZero, Dictionary<(CompoundAction, State), IEnumerable<HashSet<Model.Action>>> allDecompositions)
        {
            var newMapping = new NewSetMapping();
            var newHelper = new NewSetHelper(domain, signature.Fluents);
            foreach (var pair in resZero.Arguments)
            {
                var resultStates = resZero[pair.action, pair.state];
                allDecompositions.TryGetValue((pair.action, pair.state), out var decompositions);

                foreach (var resultState in resultStates)
                {
                    if (decompositions != null)
                        foreach (var decomposition in decompositions)
                        {
                            var action = new CompoundAction(decomposition);
                            var literals = newHelper.GetLiterals(action.Actions, pair.state, resultState);

                            newMapping[action, pair.state, resultState] = new HashSet<Literal>(literals);
                        }
                }
            }
            return newMapping;
        }
    }
}