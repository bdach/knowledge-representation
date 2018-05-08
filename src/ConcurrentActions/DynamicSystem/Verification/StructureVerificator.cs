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
    public static class StructureVerification
    {
        private static bool CheckValueStatements(List<ValueStatement> statements, TransitionFunction function, State initial)
        {
            foreach (var statement in statements)
            {
                var states = function[new CompoundAction(statement.Action), initial];
                var condition = statement.Condition;

                if (!CheckValue(states, condition))
                    return false;
            }

            return true;
        }

        private static bool CheckObservationStatements(List<ObservationStatement> statements, TransitionFunction function, State initial)
        {
            foreach (var statement in statements)
            {
                var states = function[new CompoundAction(statement.Action), initial];
                var condition = statement.Condition;

                if (!CheckObservation(states, condition))
                    return false;
            }

            return true;
        }

        private static bool CheckValue(HashSet<State> states, IFormula cond)
        {
            foreach (var state in states)
                if (!cond.Evaluate(state))
                    return false;

            return true;
        }

        private static bool CheckObservation(HashSet<State> states, IFormula cond)
        {
            foreach (var state in states)
                if (cond.Evaluate(state))
                    return true;

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
