using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing the transition function for a structure.
    /// </summary>
    public class TransitionFunction : IEnumerable<(CompoundAction action, State state, HashSet<State> resultStates)>
    {
        /// <summary>
        /// Inner dictionary mapping tuples of <see cref="CompoundAction"/>s and <see cref="State"/>s
        /// to sets of potentially resulting <see cref="State"/>s.
        /// </summary>
        private readonly Dictionary<ValueTuple<CompoundAction, State>, HashSet<State>> transitionFunction;
        private Dictionary<(CompoundAction, State), IEnumerable<HashSet<Action>>> allDecompositions;

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

        public TransitionFunction(ICollection<CompoundAction> compoundActions, ICollection<State> states, Dictionary<(CompoundAction, State), IEnumerable<HashSet<Action>>> allDecompositions) : this(compoundActions, states)
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

        /// <summary>
        /// Returns an enumerable of all argument pairs for which the transition function is defined.
        /// </summary>
        public IEnumerable<(CompoundAction action, State state)> Arguments => transitionFunction.Keys;

        /// <summary>
        /// Returns an enumerator for the transition function.
        /// The enumerator yields tuples containing transition function arguments and values.
        /// </summary>
        /// <returns>An instance of <see cref="IEnumerator{T}"/>.</returns>
        public IEnumerator<(CompoundAction action, State state, HashSet<State> resultStates)> GetEnumerator()
        {
            foreach (var entry in transitionFunction)
            {
                yield return (entry.Key.Item1, entry.Key.Item2, entry.Value);
            }
        }

        /// <summary>
        /// Returns an enumerator for the transition function.
        /// The enumerator yields tuples containing transition function arguments and values.
        /// </summary>
        /// <returns>An instance of <see cref="IEnumerator"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}