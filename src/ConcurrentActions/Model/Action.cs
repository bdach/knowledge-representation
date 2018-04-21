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
    }
}