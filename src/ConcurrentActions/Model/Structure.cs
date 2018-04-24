using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents a structure for an action language.
    /// </summary>
    public class Structure
    {
        /// <summary>
        /// A <see cref="HashSet{T}"/> of all possible <see cref="State"/>s.
        /// </summary>
        public HashSet<State> States { get; set; }

        /// <summary>
        /// The initial <see cref="State"/> in this structure.
        /// </summary>
        public State InitialState { get; set; }

        /// <summary>
        /// The <see cref="Model.TransitionFunction"/> for this structure.
        /// </summary>
        public TransitionFunction TransitionFunction { get; set; }

        /// <summary>
        /// Initializes an instance of the <see cref="Structure"/> class.
        /// </summary>
        /// <param name="states">An enumerable of all possible <see cref="State"/>s</param>
        /// <param name="initialState">The initial <see cref="State"/> in this structure.</param>
        /// <param name="transitionFunction">The <see cref="Model.TransitionFunction"/> for this structure.</param>
        /// <exception cref="ArgumentException">
        /// <see cref="initialState"/> is not present in the <see cref="states"/> enumerable.
        /// </exception>
        public Structure(IEnumerable<State> states, State initialState, TransitionFunction transitionFunction)
        {
            States = new HashSet<State>(states);
            InitialState = initialState;
            if (!States.Contains(initialState))
            {
                throw new ArgumentException(
                    "The supplied initial state is not contained in the supplied set of possible states"
                );
            }
            TransitionFunction = transitionFunction;
        }
    }
}