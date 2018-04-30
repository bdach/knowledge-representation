using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Model;
using Model.ActionLanguage;
using Model.Forms;

namespace DynamicSystem.ResZero
{
    internal class ResZero
    {
        public static IEnumerable<State> GetStates(HashSet<State> allStates, State initialState,
            ActionDomain actionDomain,
            CompoundAction compoundAction)
        {
            var resZeroStates = new List<State>();
            var actionDomainEffectStatements = actionDomain.EffectStatements;
            foreach (var action in compoundAction.Actions)
            {

//                foreach (var state in allStates)
//                {
//                    var ok = true;
//                    foreach (var effectStatement in actionDomainEffectStatements)
//                    {
//                        if(!effectStatement.Precondition.Evaluate(state))
//                            continue;
//
//                        if (!effectStatement.Postcondition.Evaluate(state))
//                        {
//                            ok = false;
//                            break;
//                        }
//
//                    }
//
//                    if (ok)
//                    {
//                        resZeroStates.Add(state);
//                    }
//                }


                var reacheableEffectStatements =
                    actionDomainEffectStatements.FindAll(effect => effect.Action.Equals(action));

                if (reacheableEffectStatements.Count == 0)
                    return new HashSet<State>() { initialState };
                
                 var conjunctedStatements = reacheableEffectStatements.Aggregate((i, j) =>
                        new EffectStatement(action,
                            new Conjunction(i.Precondition, j.Precondition),
                            new Conjunction(i.Postcondition, j.Postcondition)));

                

                var outputStates  = allStates.Where(state => conjunctedStatements.Precondition.Evaluate(initialState) &&
                                         conjunctedStatements.Postcondition.Evaluate(state));

                
                resZeroStates.AddRange(outputStates);
            }

//            return new HashSet<State>(resZeroStates);
            return resZeroStates.Count == 0 ? new HashSet<State>() {initialState} : new HashSet<State>(resZeroStates);
        }
    }
}