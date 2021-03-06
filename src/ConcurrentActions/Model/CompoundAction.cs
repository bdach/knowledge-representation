﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    /// <summary>
    /// Represents a compound action, which is a set of <see cref="Action"/>s.
    /// </summary>
    public class CompoundAction
    {
        /// <summary>
        /// The set of <see cref="Action"/>s that are part of this compound action.
        /// </summary>
        public HashSet<Action> Actions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundAction"/> class.
        /// This constructor creates an empty compound action.
        /// </summary>
        public CompoundAction()
        {
            Actions = new HashSet<Action>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundAction"/> class.
        /// The created compound action will comprise of the single atomic action supplied in the parameter.
        /// </summary>
        /// <param name="action">The atomic <see cref="Action"/> to add to the compound action.</param>
        public CompoundAction(Action action)
        {
            Actions = new HashSet<Action> {action};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundAction"/> class.
        /// The created compound action will comprise of all the actions supplied in the parameter.
        /// </summary>
        /// <param name="actions">An enumerable of atomic <see cref="Action"/>s to add to the compound action.</param>
        public CompoundAction(IEnumerable<Action> actions)
        {
            Actions = new HashSet<Action>(actions);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundAction"/> class using a collection of <see cref="Action"/>s
        /// and a collection of boolean values.
        /// </summary>
        /// <remarks>
        /// The constructor zips the two collections and preserves only the items in which the second collection has
        /// <code>true</code> values.
        /// Both collections should have the same length.
        /// </remarks>
        /// <param name="actions">The collection of <see cref="Action"/> items.</param>
        /// <param name="selection">The collection of <see cref="bool"/> items, indicating whether the action
        /// from the <see cref="actions"/> parameter should be included in the compound action.</param>
        /// <exception cref="ArgumentException">
        /// The collections of <see cref="actions"/> and <see cref="selection"/> differ in length.
        /// </exception>
        public CompoundAction(ICollection<Action> actions, ICollection<bool> selection)
        {
            if (actions.Count != selection.Count)
            {
                throw new ArgumentException("Action and selection collections have different lengths");
            }
            var selected = actions.Zip(selection, (a, s) => new Tuple<Action, bool>(a, s))
                .Where(t => t.Item2)
                .Select(t => t.Item1);
            Actions = new HashSet<Action>(selected);
        }

        public override string ToString()
        {
            var actionNames = string.Join(", ", Actions);
            return "{" + actionNames + "}";
        }

        protected bool Equals(CompoundAction other)
        {
            return Actions.SetEquals(other.Actions);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompoundAction) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (Actions != null)
                foreach (var action in Actions)
                    hashCode ^= action.GetHashCode();
            return hashCode;
        }
    }
}