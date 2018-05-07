using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents a program in the action domain (a sequence of compound actions).
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The sequence of <see cref="CompoundAction"/> objects to be performed as part of the program.
        /// </summary>
        public List<CompoundAction> Actions { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Program() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Program"/> class, using the supplied enumerable of
        /// <see cref="CompoundAction"/>s.
        /// </summary>
        /// <param name="actions">The enumerable of <see cref="CompoundAction"/>s to be performed in the program.</param>
        public Program(IEnumerable<CompoundAction> actions)
        {
            Actions = new List<CompoundAction>(actions);
        }

        public override string ToString()
        {
            var actionNames = string.Join(", ", Actions);
            return $"({actionNames})";
        }

        protected bool Equals(Program other)
        {
            return new HashSet<CompoundAction>(Actions).SetEquals(new HashSet<CompoundAction>(other.Actions));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Program) obj);
        }

        public override int GetHashCode()
        {
            var hash = 0;
            if (Actions != null)
            {
                foreach (var action in Actions) hash ^= action.GetHashCode();
            }
            return hash;
        }
    }
}