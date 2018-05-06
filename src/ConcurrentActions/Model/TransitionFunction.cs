using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Class representing the transition function for a structure.
    /// </summary>
    public class TransitionFunction
    {
        /// <summary>
        /// Inner dictionary mapping tuples of <see cref="CompoundAction"/>s and <see cref="State"/>s
        /// to sets of potentially resulting <see cref="State"/>s.
        /// </summary>
        private readonly Dictionary<ValueTuple<CompoundAction, State>, HashSet<State>> transitionFunction;

        /// <summary>
        /// Property for accessing compound actions that are used within a function domain.
        /// </summary>
        public ICollection<CompoundAction> CompoundActions { get; }

        /// <summary>
        /// Property for accessing states that are used within a function domain.
        /// </summary>
        public ICollection<State> States { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionFunction"/> class.
        /// </summary>
        /// <param name="compoundActions">
        /// A collection of <see cref="CompoundAction"/>s to be considered in the function domain.
        /// </param>
        /// <param name="states">
        /// A collection of <see cref="State"/>s in the structure to be considered in the function domain.
        /// </param>
        public TransitionFunction(ICollection<CompoundAction> compoundActions, ICollection<State> states)
        {
            CompoundActions = compoundActions;
            States = states;

            transitionFunction = new Dictionary<(CompoundAction, State), HashSet<State>>();
            foreach (var compoundAction in compoundActions)
            {
                foreach (var state in states)
                {
                    transitionFunction[ValueTuple.Create(compoundAction, state)] = new HashSet<State>();
                }
            }
        }

        /// <summary>
        /// Accessor property that allows to fetch and modify the values of the transition function
        /// for the supplied <see cref="CompoundAction"/> and <see cref="State"/>.
        /// </summary>
        /// <param name="compoundAction">The <see cref="CompoundAction"/> to fetch the function value for.</param>
        /// <param name="state">The <see cref="State"/> to fetch the function value for.</param>
        /// <returns>
        /// A <see cref="HashSet{T}"/> of <see cref="State"/>s which represents the value of the transition function.
        /// </returns>
        public HashSet<State> this[CompoundAction compoundAction, State state]
        {
            get => transitionFunction[ValueTuple.Create(compoundAction, state)];
            set => transitionFunction[ValueTuple.Create(compoundAction, state)] = value;
        }

        //Possible implementation of GetAllCompoundActionsForState(State state)
        //It would need State.Equals to be overriden
        /// <summary>
        /// Returns a list of <see cref="CompoundAction"/> for a given state
        /// </summary>
        //public List<CompoundAction> GetAllCompoundActionsForState(State state)
        //{
        //    List<CompoundAction> result = new List<CompoundAction>();
        //    foreach (var key in transitionFunction.Keys)
        //    {
        //        if (key.Item2.Equals(state))
        //        {
        //            result.Add(key.Item1);
        //        }
        //    }
        //    return result;
        //}
    }
}