using System.Collections.Generic;
using Model;
using Model.Forms;

namespace DynamicSystem.NewGeneration
{
    /// <summary>
    /// Helper class used to hold the New mapping values.
    /// </summary>
    public class NewSetMapping
    {
        /// <summary>
        /// Internal backing dictionary.
        /// </summary>
        private Dictionary<(CompoundAction, State, State), HashSet<Literal>> newMapping;

        /// <summary>
        /// Constructs a new instance of the <see cref="NewSetMapping"/> class.
        /// </summary>
        public NewSetMapping()
        {
            newMapping = new Dictionary<(CompoundAction, State, State), HashSet<Literal>>();
        }

        /// <summary>
        /// Accessor property used to set and fetch values of the mapping.
        /// </summary>
        /// <param name="compoundAction">The <see cref="CompoundAction"/> for which to fetch the New set.</param>
        /// <param name="initialState">The initial <see cref="State"/> for which to fetch the New set.</param>
        /// <param name="exitState">The exit <see cref="State"/> for which to fetch the New set.</param>
        /// <returns>A <see cref="HashSet{T}"/> containing all the <see cref="Literal"/>s which change between the initial and exit states.</returns>
        public HashSet<Literal> this[CompoundAction compoundAction, State initialState, State exitState]
        {
            get => newMapping[(compoundAction, initialState, exitState)];
            set => newMapping[(compoundAction, initialState, exitState)] = value;
        }

        public bool KeyExists((CompoundAction compoundAction, State initialState, State exitState) key)
        {
            return newMapping.ContainsKey(key);
        }

        /// <summary>
        /// Returns all values of the mapping.
        /// </summary>
        public ICollection<HashSet<Literal>> AllValues => newMapping.Values;
    }
}