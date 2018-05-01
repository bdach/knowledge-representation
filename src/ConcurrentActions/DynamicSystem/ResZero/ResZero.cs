using System.Collections.Generic;
using System.Linq;
using Model;
using Model.ActionLanguage;
using Model.Forms;

namespace DynamicSystem.ResZero
{
    /// <inheritdoc />
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
        /// <param name="compoundAction">Actions that are to be performed on given state</param>
        /// <returns>Iterable collection of <see cref="State"/>, which is the result of ResZero </returns>
        public IEnumerable<State> GetStates(State initialState, CompoundAction compoundAction)
        {
            var resZeroStates = new List<State>();
            foreach (var action in compoundAction.Actions)
            {
                
                var reacheableEffectStatements =
                    _effectStatement.FindAll(effect => effect.Action.Equals(action));

                if (reacheableEffectStatements.Count == 0)
                    return new HashSet<State>(_allStates);

                var conjunctedStatements = reacheableEffectStatements.Aggregate((sentenceOne, sentenceTwo) =>
                    new EffectStatement(action,
                        new Conjunction(sentenceOne.Precondition, sentenceTwo.Precondition),
                        new Conjunction(sentenceOne.Postcondition, sentenceTwo.Postcondition)));


                var outputStates = _allStates.Where(state => conjunctedStatements.Precondition.Evaluate(initialState) &&
                                                            conjunctedStatements.Postcondition.Evaluate(state));


                resZeroStates.AddRange(outputStates);
            }
            return resZeroStates.Count == 0 ? new HashSet<State>() {initialState} : new HashSet<State>(resZeroStates);
        }
    }
}