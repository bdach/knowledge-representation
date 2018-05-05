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
    public static class TransitionFunctionGenerator
    {

        /// <summary>
        /// Generates <see cref="TransitionFunction"/> by finding states belonging to output of Res_0 function 
        /// that minimalize New set of inertial fluent that can change with action execution.
        /// </summary>
        /// <param name="resZero"><see cref="Dictionary{ValueTuple{CompoundAction, State}, HashSet{State}"/> describing Res_0 function.</param>
        /// <param name="newSets"><see cref="Dictionary{ValueTuple{CompoundAction, State, State}, HashSet{Fluent}}"/> describing New sets.</param>
        /// <returns><see cref="TransitionFunction"/> object.</returns>
        public static TransitionFunction GenerateTransitionFunction(
            Dictionary<(CompoundAction, State), HashSet<State>> resZero,
            Dictionary<(CompoundAction, State, State), HashSet<Fluent>> newSets)
        {
            var transitionFunction = InitializeTransitionFunction(resZero);

            foreach (var assignment in resZero)
            {
                var compoundAction = assignment.Key.Item1;
                var state = assignment.Key.Item2;
                var potentialResults = assignment.Value;

                var consideredNewSets = FindConsideredNewSets(compoundAction, state, potentialResults, newSets);

                transitionFunction[compoundAction, state] =
                    GenerateTransitionFunction((compoundAction, state, potentialResults), consideredNewSets);
            }
            return transitionFunction;
        }

        /// <summary>
        /// Initializes <see cref="TransitionFunction"/> with compound actions and states which are arguments of Res_0 function.
        /// </summary>
        /// <param name="resZero"><see cref="Dictionary{ValueTuple{CompoundAction, State}, HashSet{State}"/> describing Res_0 function.</param>
        /// <returns>Empty <see cref="TransitionFunction"/>.</returns>
        private static TransitionFunction InitializeTransitionFunction(Dictionary<(CompoundAction, State), HashSet<State>> resZero)
        {
            var compoundActions = resZero.Keys
                .Select(key => key.Item1)
                .ToArray();
            var states = resZero.Keys
                .Select(key => key.Item2)
                .ToArray();

            return new TransitionFunction(compoundActions, states);
        }

        /// <summary>
        /// Filter <see cref="HashSet{State}"/> New sets that will be considered for specific <see cref="CompoundAction"/> and <see cref="State"/>
        /// </summary>
        /// <param name="compoundAction"><see cref="CompoundAction"/> to be executed.</param>
        /// <param name="state">Current <see cref="State"/> of a dynamic system.</param>
        /// <param name="potentialResults"><see cref="HashSet{StateT}"/> with possible results of compound action execution.</param>
        /// <param name="newSets"><see cref="Dictionary{ValueTuple{CompoundAction, State, State}, HashSet{Fluent}}"/> describing New sets.</param>
        /// <returns><see cref="Dictionary{ValueTuple{CompoundAction, State, State}, HashSet{Fluent}}"/> containing New sets that will be considered.</returns>
        private static Dictionary<(CompoundAction, State, State), HashSet<Fluent>> FindConsideredNewSets(
            CompoundAction compoundAction, State state, HashSet<State> potentialResults,
            Dictionary<(CompoundAction, State, State), HashSet<Fluent>> newSets)
        {
            return newSets
                .Where(newSet =>
                       compoundAction == newSet.Key.Item1
                    && state == newSet.Key.Item2
                    && potentialResults.Contains(newSet.Key.Item3))
                .ToDictionary(newSet => newSet.Key, newSet => newSet.Value);
        }

        /// <summary>
        /// Generates transition function for specific compound action and state by finding states belonging to output of Res_0 function 
        /// that minimalize New set of inertial fluent that can change with action execution.
        /// </summary>
        /// <param name="assignment"><see cref="ValueTuple{CompoundAction, State, HashSet{State}}"/> describing Res_0 assignment.</param>
        /// <param name="newSetDict"><see cref="Dictionary{ValueTuple{CompoundAction, State, State}, HashSet{Fluent}}"/> with New sets that are considered.</param>
        /// <returns></returns>
        private static HashSet<State> GenerateTransitionFunction(
            (CompoundAction, State, HashSet<State>) assignment,
            Dictionary<(CompoundAction, State, State), HashSet<Fluent>> newSetDict)
        {
            var compoundAction = assignment.Item1;
            var state = assignment.Item2;
            var potentialResults = assignment.Item3;

            var newSets = newSetDict.Values;
            var minimalizingStates = new HashSet<State>();

            foreach (var result in potentialResults)
            {
                var resultNewSet = newSetDict[(compoundAction, state, result)];
                if (!newSets.Any(newSet => newSet.IsProperSubsetOf(resultNewSet)))
                    minimalizingStates.Add(result);
            }
            return minimalizingStates;
        }
    }
}
