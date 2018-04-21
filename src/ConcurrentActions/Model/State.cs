using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    /// <summary>
    /// Represents a mapping from the set of <see cref="Fluent"/>s to logical values.
    /// A state assigns values to the elements of the language signature.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Accessor property that allows to fetch and modify the values of <see cref="Fluent"/>s in the current state instance.
        /// </summary>
        /// <param name="fluent">The fluent whose state should be returned or modified.</param>
        /// <returns>The value associated with the specified fluent.</returns>
        /// <exception cref="ArgumentException">The supplied fluent is not defined in this state.</exception>
        bool this[Fluent fluent] { get; set; }
    }

    /// <inheritdoc />
    public class State : IState
    {
        /// <summary>
        /// Dictionary mapping the particular <see cref="Fluent"/>s to their binary values.
        /// </summary>
        private readonly Dictionary<Fluent, bool> fluentState;

        /// <summary>
        /// Initializes a new instance of the <see cref="Fluent"/> class, using the supplied dictionary.
        /// </summary>
        /// <param name="fluentState">A dictionary mapping <see cref="Fluent"/>s to their binary values.</param>
        public State(IDictionary<Fluent, bool> fluentState)
        {
            this.fluentState = new Dictionary<Fluent, bool>(fluentState);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fluent"/> class, using <see cref="ICollection{T}"/>s of <see cref="Fluent"/>s and their values.
        /// </summary>
        /// <remarks>
        /// The <see cref="fluents"/> and <see cref="values"/> collections should have the same count of elements.
        /// </remarks>
        /// <param name="fluents">A <see cref="ICollection{T}"/> of <see cref="Fluent"/> objects.</param>
        /// <param name="values">A <see cref="ICollection{T}"/> of boolean values to be assigned to each of the <see cref="fluents"/>.</param>
        /// <exception cref="ArgumentException">The collections of <see cref="fluents"/>and their <see cref="values"/> differ in length.</exception>
        public State(ICollection<Fluent> fluents, ICollection<bool> values)
        {
            if (fluents.Count != values.Count)
            {
                throw new ArgumentException("Fluent and value lists have different lengths");
            }
            fluentState = fluents.Zip(values, (f, v) => new KeyValuePair<Fluent, bool>(f, v))
                .ToDictionary(p => p.Key, p => p.Value);
        }

        /// <inheritdoc />
        public bool this[Fluent fluent]
        {
            get => GetFluentState(fluent);
            set => SetFluentState(fluent, value);
        }

        private void SetFluentState(Fluent fluent, bool value)
        {
            if (fluentState.ContainsKey(fluent))
            {
                fluentState[fluent] = value;
            }
            else
            {
                throw new ArgumentException($"Attempted to set state of unknown fluent {fluent}");
            }
        }

        private bool GetFluentState(Fluent fluent)
        {
            try
            {
                return fluentState[fluent];
            }
            catch (KeyNotFoundException ex)
            {
                throw new ArgumentException($"Attempted to get state of unknown fluent {fluent}", ex);
            }
        }
    }
}