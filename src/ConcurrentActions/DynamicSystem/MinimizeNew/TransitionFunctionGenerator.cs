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
        public TransitionFunction GenerateTransitionFunction(
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

        private HashSet<State> GenerateTransitionFunction(
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
