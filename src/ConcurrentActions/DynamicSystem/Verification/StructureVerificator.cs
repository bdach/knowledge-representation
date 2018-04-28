using Model;
using Model.ActionLanguage;
using Model.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicSystem.Verification
{
    public  static class StructureVerification
    {
        private static bool CheckValueStatements(List<ValueStatement> statements, TransitionFunction function, State initial)
        {
            
            foreach(var statement in statements)
            {
                
                var condition = statement.Condition;
                var compoundAction = new CompoundAction(statement.Action);
                var states = new HashSet<State> { initial };

                if (!CheckValue(states, compoundAction, condition, function))
                    return false;
            }
            
            return true;
        }

        private static bool CheckObservationStatements(List<ObservationStatement> statements, TransitionFunction function, State initial)
        {
            foreach (var statement in statements)
            {

                var condition = statement.Condition;
                var compoundAction = new CompoundAction(statement.Action);
                var states = new HashSet<State> { initial };

                if (!CheckObservation(states, compoundAction, condition, function))
                    return false;
            }

            return true;
        }

        private static bool CheckValue(HashSet<State> states, CompoundAction action, IFormula cond, TransitionFunction function)
        {
            foreach (var state in states)
            {
                if (cond.Evaluate(state) == true)
                    continue;

                var nextStates = function[action, state];
                if (nextStates.Count == 0)
                    return false;

                if (!CheckValue(nextStates, action, cond, function))
                    return false;
            }

            return true;
        }

        private static bool CheckObservation(HashSet<State> states, CompoundAction action, IFormula cond, TransitionFunction function)
        {
            foreach (var state in states)
            {
                if (cond.Evaluate(state) == true)
                    return true;

                var nextStates = function[action, state];
                if (nextStates.Count == 0)
                    continue;

                if (CheckValue(nextStates, action, cond, function))
                    return true;
            }

            return false;
        }

        public static bool CheckStatements(ActionDomain domain, Structure structure)
        {
            var valueStatements = domain.ValueStatements;
            var observationStatements = domain.ObservationStatements;

            return CheckValueStatements(valueStatements, structure.TransitionFunction, structure.InitialState) 
                && CheckObservationStatements(observationStatements, structure.TransitionFunction, structure.InitialState);
        }
    }
}
