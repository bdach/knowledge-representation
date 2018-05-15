﻿using System.Collections.Generic;
using Model;
using Model.ActionLanguage;
using Model.Forms;

namespace DynamicSystem.NewGeneration
{
    public static class NewSetGenerator
    {
        // TODO: I don't like this return type too much, but I'm gluing all of this as-is
        public static Dictionary<(CompoundAction, State, State), HashSet<Literal>> GetNewSets(ActionDomain domain, Signature signature, TransitionFunction resZero)
        {
            var newMapping = new Dictionary<(CompoundAction, State, State), HashSet<Literal>>();
            var newHelper = new NewSetHelper(domain, signature.Fluents);
            foreach (var pair in resZero.Arguments)
            {
                var resultStates = resZero[pair.action, pair.state];
                foreach (var resultState in resultStates)
                {
                    var newValue = newHelper.GetLiterals(pair.action.Actions, pair.state, resultState);
                    newMapping[(pair.action, pair.state, resultState)] = new HashSet<Literal>(newValue);
                }
            }
            return newMapping;
        }
    }
}