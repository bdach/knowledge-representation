using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents a single fluent in the dynamic system.
    /// A fluent can be equated to a binary state of a particular element of the language signature.
    /// </summary>
    public class Fluent
    {
        /// <summary>
        /// Name of the fluent.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public Fluent() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fluent"/> class with a specified name.
        /// </summary>
        /// <param name="name">The name of the fluent.</param>
        public Fluent(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Fluent fluent && Name == fluent.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}