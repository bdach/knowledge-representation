namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single fluent specification statement.
    /// The fluent specification statement declares that a fluent is non-inertial.
    /// </summary>
    public class FluentSpecificationStatement
    {
        /// <summary>
        /// The <see cref="Model.Fluent"/> associated with the statement.
        /// </summary>
        public Fluent Fluent { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public FluentSpecificationStatement() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentSpecificationStatement"/> class.
        /// </summary>
        /// <param name="fluent">The <see cref="Model.Fluent"/> associated with the statement.</param>
        public FluentSpecificationStatement(Fluent fluent)
        {
            Fluent = fluent;
        }

        public override string ToString()
        {
            return $"noninertial {Fluent}";
        }
    }
}