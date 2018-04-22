using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single value statement in the action domain.
    /// </summary>
    public class ValueStatement
    {
        /// <summary>
        /// The <see cref="IFormula"/> instance representing the condition which must be satisfied upon completing the associated <see cref="Model.Action"/>.
        /// </summary>
        public IFormula Condition { get; set; }

        /// <summary>
        /// The <see cref="Model.Action"/> associated with the statement.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueStatement"/> class.
        /// </summary>
        /// <param name="condition">The <see cref="IFormula"/> instance representing the condition which must be satisfied upon completing the associated <see cref="Model.Action"/>.</param>
        /// <param name="action">The <see cref="Model.Action"/> associated with the statement.</param>
        public ValueStatement(IFormula condition, Action action)
        {
            Condition = condition;
            Action = action;
        }

        public override string ToString()
        {
            return $"{Condition} after {Action}";
        }
    }
}