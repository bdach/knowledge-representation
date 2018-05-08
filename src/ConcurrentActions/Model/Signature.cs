using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// Represents the signature of an action language.
    /// </summary>
    public class Signature
    {
        /// <summary>
        /// Set of all fluents in the signature.
        /// </summary>
        public HashSet<Fluent> Fluents { get; set; }
        
        /// <summary>
        /// Set of all actions in the signature.
        /// </summary>
        public HashSet<Action> Actions { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="Signature"/> class.
        /// </summary>
        /// <param name="fluents">An <see cref="IEnumerable{T}"/> of <see cref="Fluent"/> objects.</param>
        /// <param name="actions">An <see cref="IEnumerable{T}"/> of <see cref="Action"/> objects.</param>
        public Signature(IEnumerable<Fluent> fluents, IEnumerable<Action> actions)
        {
            Fluents = new HashSet<Fluent>(fluents);
            Actions = new HashSet<Action>(actions);
        }
    }
}