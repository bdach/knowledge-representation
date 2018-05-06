using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using DynamicSystem.DNF;
using Model;
using Model.Forms;
using Model.QueryLanguage;

namespace DynamicSystem.QueriesEvaluation
{
    /// <summary>
    /// Class for evaluating queries for all domain models.
    /// </summary>
    public static class QueriesEvaluator
    {
        /// <summary>
        /// Evaluates a <see cref="ExistentialExecutabilityQuery"/>
        /// </summary>
        public static bool EvaluateExistentialExecutabilityQuery(HashSet<State> initialStates, TransitionFunction transitionFunction, ExistentialExecutabilityQuery query)
        {
            return ExistentialExecutabilityQueryRecursion(initialStates, transitionFunction, query.Program.Actions, 0);
        }

        private static bool ExistentialExecutabilityQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex)
        {
            if (possibleStates.Count == 0)
            {
                return false;
            }
            if (actions.Count <= currentActionIndex)
            {
                return true;
            }
            var currentAction = actions[currentActionIndex];
            foreach (var state in possibleStates)
            {
                HashSet<State> newPossibleStates = transitionFunction[currentAction, state];
                bool result = ExistentialExecutabilityQueryRecursion(newPossibleStates, transitionFunction, actions, currentActionIndex + 1);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evaluates a <see cref="GeneralExecutabilityQuery"/>
        /// </summary>
        public static bool EvaluateGeneralExecutabilityQuery(HashSet<State> initialStates, TransitionFunction transitionFunction, GeneralExecutabilityQuery query)
        {
            return GeneralExecutabilityQueryRecursion(initialStates, transitionFunction, query.Program.Actions, 0);
        }

        private static bool GeneralExecutabilityQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex)
        {
            if (possibleStates.Count == 0)
            {
                return false;
            }
            if (actions.Count <= currentActionIndex)
            {
                return true;
            }
            var currentAction = actions[currentActionIndex];
            foreach (var state in possibleStates)
            {
                HashSet<State> newPossibleStates = transitionFunction[currentAction, state];
                bool result = GeneralExecutabilityQueryRecursion(newPossibleStates, transitionFunction, actions, currentActionIndex + 1);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Evaluates a <see cref="ExistentialValueQuery"/>
        /// </summary>
        public static bool EvaluateExistentialValueQuery(HashSet<State> initialStates, TransitionFunction transitionFunction, ExistentialValueQuery query)
        {
            return ExistentialValueQueryRecursion(initialStates, transitionFunction, query.Program.Actions, 0, query.Target);
        }

        private static bool ExistentialValueQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex, IFormula formula)
        {
            if (actions.Count == currentActionIndex)
            {
                foreach (var state in possibleStates)
                {
                    if (formula.Evaluate(state))
                    {
                        return true;
                    }
                }
                return false;
            }

            var currentAction = actions[currentActionIndex];
            foreach (var state in possibleStates)
            {
                HashSet<State> newPossibleStates = transitionFunction[currentAction, state];
                bool result = ExistentialValueQueryRecursion(newPossibleStates, transitionFunction, actions, currentActionIndex + 1, formula);
                if (result)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Evaluates a <see cref="GeneralValueQuery"/>
        /// </summary>
        public static bool EvaluateGeneralValueQuery(HashSet<State> initialStates, TransitionFunction transitionFunction, GeneralValueQuery query)
        {
            return GeneralValueQueryRecursion(initialStates, transitionFunction, query.Program.Actions, 0, query.Target);
        }

        private static bool GeneralValueQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex, IFormula formula)
        {
            if (actions.Count == currentActionIndex)
            {
                foreach (var state in possibleStates)
                {
                    if (!formula.Evaluate(state))
                    {
                        return false;
                    }
                }
                return true;
            }

            var currentAction = actions[currentActionIndex];
            foreach (var state in possibleStates)
            {
                HashSet<State> newPossibleStates = transitionFunction[currentAction, state];
                bool result = ExistentialValueQueryRecursion(newPossibleStates, transitionFunction, actions, currentActionIndex + 1, formula);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///Evaluates a <see cref="AccessibilityQuery"/>
        /// </summary>
        public static bool EvaluateAccessibilityQuery(HashSet<State> initialStates, TransitionFunction transitionFunction, AccessibilityQuery query)
        {
            HashSet<State> visitedStates = new HashSet<State>();
            foreach (var state in initialStates)
            {
                bool result = AccessibilityQueryRecursion(visitedStates, state, transitionFunction, query);
                if (!result)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool AccessibilityQueryRecursion(HashSet<State> visitedStates, State currentState, TransitionFunction transitionFunction, AccessibilityQuery query)
        {
            if (query.Target.Evaluate(currentState))
            {
                return true;
            }
            if (visitedStates.Contains(currentState))
            {
                return false;
            }

            HashSet<State> newVisitedStates = new HashSet<State>(visitedStates);
            newVisitedStates.Add(currentState);

            List<CompoundAction> possibleActions = transitionFunction.CompoundActions.Where(
                (compoundAction) => transitionFunction[compoundAction, currentState] != null &&
                                    transitionFunction[compoundAction, currentState].Count > 0).ToList();

            if (!possibleActions.Any())
            {
                return false;
            }

            
            bool atLeastOneActionLeadsToPositiveEnding = false;
            foreach (var action in possibleActions)
            {
                HashSet<State> possibleStates = transitionFunction[action, currentState];

                bool fromEveryStateThereIsAPathToPositiveEnding = true;
                bool atLeastOneState = false;

                foreach (var state in possibleStates)
                {
                    if (state == currentState)
                    {
                        continue;
                    }
                    atLeastOneState = true;
                    bool result = AccessibilityQueryRecursion(newVisitedStates, state, transitionFunction, query);
                    if (!result)
                    {
                        fromEveryStateThereIsAPathToPositiveEnding = false;
                        break;
                    }
                }

                if (fromEveryStateThereIsAPathToPositiveEnding && atLeastOneState)
                {
                    atLeastOneActionLeadsToPositiveEnding = true;
                    break;
                }
            }

            if (atLeastOneActionLeadsToPositiveEnding)
            {
                return true;
            }
            return false;   
        }
    }
}
