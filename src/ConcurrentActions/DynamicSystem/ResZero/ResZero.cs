using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ActionLanguage;

namespace DynamicSystem.ResZero
{
    /// <summary>
    /// Class for generating states that are the output of Res Zero.
    /// </summary>
    internal class ResZero
    {
        private readonly List<EffectStatement> _effectStatement;
        private readonly HashSet<State> _allStates;

        /// <summary>
        /// Constructor taking initial information about the project domain.
        /// </summary>
        /// <param name="effectStatement">List of Effect Statement (Causes) taken from <see cref="ActionDomain"/>.</param>
        /// <param name="allStates">HashSet of all possible <see cref="State"/> available from <see cref="Structure"/> class</param>
        public ResZero(List<EffectStatement> effectStatement, HashSet<State> allStates)
        {
            _effectStatement = effectStatement;
            _allStates = allStates;
        }

        /// <summary>
        /// Generate states that are the result of performing a compound action on initial state in project domain
        /// </summary>
        /// <param name="initialState">State on which ResZero will perform.</param>
        /// <param name="decomposition">The decomposition to be performed in a given state</param>
        /// <returns>Iterable collection of <see cref="State"/>, which is the result of ResZero </returns>
        public IEnumerable<State> GetStates(State initialState, CompoundAction decomposition)
        {
            var statementsOnAction =
                _effectStatement.FindAll(effect => decomposition.Actions.Contains(effect.Action));
            if (statementsOnAction.Count == 0)
            {
                return new HashSet<State>(_allStates);
            }
            var reachableStatements =
                statementsOnAction.FindAll(statement => statement.Precondition.Evaluate(initialState));
            var accessibleStates = _allStates.Where(state =>
                reachableStatements.All(statement => statement.Postcondition.Evaluate(state)));
            return new HashSet<State>(accessibleStates);
        }
    }
}