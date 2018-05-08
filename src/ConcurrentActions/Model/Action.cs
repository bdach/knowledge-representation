using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents a single action in the dynamic system.
    /// An action represents some kind of activity in the system being described, with certain or uncertain effects.
    /// </summary>
    public class Action
    {
        /// <summary>
        /// Name of the action.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Action() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Action"/> class with the supplied name.
        /// </summary>
        /// <param name="name">The name of the action.</param>
        public Action(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Action action && Name == action.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}