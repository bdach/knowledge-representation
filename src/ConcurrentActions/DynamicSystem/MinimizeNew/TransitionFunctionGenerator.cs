using DynamicSystem.NewGeneration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = Model.Action;

namespace DynamicSystem.MinimizeNew
{
    /// <summary>
    /// Class for generating transition function
    /// </summary>
    public static class TransitionFunctionGenerator
    {
        [Obsolete]
        public static TransitionFunction GenerateTransitionFunction(TransitionFunction resZero, NewSetMapping newSets)
        {
            var transitionFunction = InitializeTransitionFunction(resZero);

            foreach (var assignment in resZero)
            {
                var (compoundAction, state, potentialResults) = assignment;
                var consideredNewSets = FindConsideredNewSets(compoundAction, state, potentialResults, newSets);

                transitionFunction[compoundAction, state] =
                    GenerateTransitionFunction((compoundAction, state, potentialResults), consideredNewSets);
            }
            return transitionFunction;
        }

        /// <summary>
        /// Generates <see cref="TransitionFunction"/> by finding states belonging to output of Res_0 function 
        /// that minimalize New set of inertial fluent that can change with action execution.
        /// </summary>
        /// <param name="resZero"><see cref="TransitionFunction"/> instance describing Res_0 function.</param>
        /// <param name="newSets"><see cref="NewSetMapping"/> describing New sets.</param>
        /// <param name="allDecompositions">Decompositions</param>
        /// <returns><see cref="TransitionFunction"/> object.</returns>
        public static TransitionFunction GenerateTransitionFunction(TransitionFunction resZero, NewSetMapping newSets,
            Dictionary<(CompoundAction, State), IEnumerable<HashSet<Action>>> allDecompositions)
        {
            var transitionFunction = InitializeTransitionFunction(resZero);

            foreach (var assignment in resZero)
            {
                var (compoundAction, state, potentialResults) = assignment;

                allDecompositions.TryGetValue((compoundAction, state), out var decompositions);

                HashSet<State> states = new HashSet<State>();

                foreach (var decomposition in decompositions)
                {
                    var action = new CompoundAction(decomposition);
                    var consideredNewSets = FindConsideredNewSets(action, state, resZero[action, state], newSets);

                    var generatedTransition = GenerateTransitionFunction((action, state, resZero[action, state]), consideredNewSets);
                    states.UnionWith(generatedTransition);
                }

                transitionFunction[compoundAction, state] = states;
            }

            return transitionFunction;
        }

        /// <summary>
        /// Initializes <see cref="TransitionFunction"/> with compound actions and states which are arguments of Res_0 function.
        /// </summary>
        /// <param name="resZero"><see cref="TransitionFunction"/> instance describing Res_0 function.</param>
        /// <returns>Empty <see cref="TransitionFunction"/>.</returns>
        private static TransitionFunction InitializeTransitionFunction(TransitionFunction resZero)
        {
            return new TransitionFunction(resZero.CompoundActions, resZero.States);
        }

        /// <summary>
        /// Filter <see cref="HashSet{State}"/> New sets that will be considered for specific <see cref="CompoundAction"/> and <see cref="State"/>
        /// </summary>
        /// <param name="compoundAction"><see cref="CompoundAction"/> to be executed.</param>
        /// <param name="state">Current <see cref="State"/> of a dynamic system.</param>
        /// <param name="potentialResults"><see cref="HashSet{StateT}"/> with possible results of compound action execution.</param>
        /// <param name="newSets"><see cref="NewSetMapping"/> describing New sets.</param>
        /// <returns><see cref="NewSetMapping"/> containing New sets that will be considered.</returns>
        private static NewSetMapping FindConsideredNewSets(CompoundAction compoundAction, State state, HashSet<State> potentialResults, NewSetMapping newSets)
        {
            var consideredNewSets = new NewSetMapping();
            foreach (var result in potentialResults)
            {
                consideredNewSets[compoundAction, state, result] = newSets[compoundAction, state, result];
            }
            return consideredNewSets;
        }

        /// <summary>
        /// Generates transition function for specific compound action and state by finding states belonging to output of Res_0 function 
        /// that minimalize New set of inertial fluent that can change with action execution.
        /// </summary>
        /// <param name="assignment"><see cref="ValueTuple{CompoundAction, State, HashSet{State}}"/> describing Res_0 assignment.</param>
        /// <param name="newSetDict"><see cref="NewSetMapping"/> with New sets that are considered.</param>
        /// <returns></returns>
        private static HashSet<State> GenerateTransitionFunction((CompoundAction, State, HashSet<State>) assignment,
            NewSetMapping newSetDict)
        {
            var compoundAction = assignment.Item1;
            var state = assignment.Item2;
            var potentialResults = assignment.Item3;    

            var newSets = newSetDict.AllValues;
            var minimalizingStates = new HashSet<State>();

            foreach (var result in potentialResults)
            {
                var resultNewSet = newSetDict[compoundAction, state, result];
                if (!newSets.Any(newSet => newSet.IsProperSubsetOf(resultNewSet)))
                    minimalizingStates.Add(result);
            }
            return minimalizingStates;
        }
    }
}
