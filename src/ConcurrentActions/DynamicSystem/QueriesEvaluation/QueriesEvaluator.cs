using System.Collections.Generic;
using System.Linq;
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
        /// Performs evaluation of queries for all supplied models.
        /// </summary>
        /// <param name="models">Collection of <see cref="Structure"/> instances representing the models.</param>
        /// <param name="queries"><see cref="QuerySet"/> instance representing the set of queries.</param>
        /// <returns><see cref="QueryResolution"/> object containing the results of the queries.</returns>
        public static QueryResolution EvaluateQueries(ICollection<Structure> models, QuerySet queries)
        {
            var queryResolution = new QueryResolution();

            foreach (var query in queries.AccessibilityQueries)
            {
                queryResolution.AccessibilityQueryResults.Add((query, EvaluateAccessibilityQuery(models, query)));
            }
            foreach (var query in queries.ExistentialExecutabilityQueries)
            {
                queryResolution.ExistentialExecutabilityQueryResults.Add((query, EvaluateExistentialExecutabilityQuery(models, query)));
            }
            foreach (var query in queries.ExistentialValueQueries)
            {
                queryResolution.ExistentialValueQueryResults.Add((query, EvaluateExistentialValueQuery(models, query)));
            }
            foreach (var query in queries.GeneralExecutabilityQueries)
            {
                queryResolution.GeneralExecutabilityQueryResults.Add((query, EvaluateGeneralExecutabilityQuery(models, query)));
            }
            foreach (var query in queries.GeneralValueQueries)
            {
                queryResolution.GeneralValueQueryResults.Add((query, EvaluateGeneralValueQuery(models, query)));
            }
            return queryResolution;
        }

        /// <summary>
        /// Evaluates a <see cref="ExistentialExecutabilityQuery"/>
        /// </summary>
        public static bool EvaluateExistentialExecutabilityQuery(ICollection<Structure> models, ExistentialExecutabilityQuery query)
        {
            return models.All(model => ExistentialExecutabilityQueryRecursion(
                new HashSet<State> {model.InitialState},
                model.TransitionFunction,
                query.Program.Actions,
                0
            ));
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
        public static bool EvaluateGeneralExecutabilityQuery(ICollection<Structure> models, GeneralExecutabilityQuery query)
        {
            return models.All(model => GeneralExecutabilityQueryRecursion(
                new HashSet<State> {model.InitialState},
                model.TransitionFunction,
                query.Program.Actions,
                0
            ));
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
        public static bool EvaluateExistentialValueQuery(ICollection<Structure> models, ExistentialValueQuery query)
        {
            return models.All(model => ExistentialValueQueryRecursion(
                new HashSet<State> {model.InitialState},
                model.TransitionFunction,
                query.Program.Actions,
                0,
                query.Target
            ));
        }

        private static bool ExistentialValueQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex, IFormula formula)
        {
            if (possibleStates.Count == 0)
            {
                return false;
            }
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
        public static bool EvaluateGeneralValueQuery(ICollection<Structure> models, GeneralValueQuery query)
        {
            return models.All(model => GeneralValueQueryRecursion(
                new HashSet<State> {model.InitialState},
                model.TransitionFunction,
                query.Program.Actions,
                0,
                query.Target
            ));
        }

        private static bool GeneralValueQueryRecursion(HashSet<State> possibleStates, TransitionFunction transitionFunction, List<CompoundAction> actions, int currentActionIndex, IFormula formula)
        {
            if (possibleStates.Count == 0)
            {
                return false;
            }
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
                bool result = GeneralValueQueryRecursion(newPossibleStates, transitionFunction, actions, currentActionIndex + 1, formula);
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
        public static bool EvaluateAccessibilityQuery(ICollection<Structure> models, AccessibilityQuery query)
        {
            HashSet<State> visitedStates = new HashSet<State>();
            return models.All(model => AccessibilityQueryRecursion(
                visitedStates,
                model.InitialState,
                model.TransitionFunction,
                query
            ));
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

                bool thereIsAPathToPositiveEnding = false;
                bool atLeastOneState = false;

                foreach (var state in possibleStates)
                {
                    if (state.Equals(currentState))
                    {
                        continue;
                    }
                    atLeastOneState = true;
                    bool result = AccessibilityQueryRecursion(newVisitedStates, state, transitionFunction, query);
                    if (result)
                    {
                        thereIsAPathToPositiveEnding = true;
                        break;
                    }
                }

                if (thereIsAPathToPositiveEnding && atLeastOneState)
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
