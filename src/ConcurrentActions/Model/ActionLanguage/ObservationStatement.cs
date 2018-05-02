using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single observation statement in the action domain.
    /// </summary>
    public class ObservationStatement
    {
        /// <summary>
        /// The <see cref="IFormula"/> instance representing the condition which should be satisfied on at least one execution path upon completing the associated <see cref="Model.Action"/>.
        /// </summary>
        public IFormula Condition { get; set; }

        /// <summary>
        /// The <see cref="Model.Action"/> associated with the statement.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public ObservationStatement() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationStatement"/> class.
        /// </summary>
        /// <param name="condition">The <see cref="IFormula"/> instance representing the condition which should be satisfied on at least one execution path upon completing the associated <see cref="Model.Action"/>.</param>
        /// <param name="action">The <see cref="Model.Action"/> associated with the statement.</param>
        public ObservationStatement(IFormula condition, Action action)
        {
            Condition = condition;
            Action = action;
        }

        public override string ToString()
        {
            return $"observable {Condition} after {Action}";
        }
    }
}