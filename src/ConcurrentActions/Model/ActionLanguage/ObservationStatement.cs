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

        protected bool Equals(ObservationStatement other)
        {
            return Equals(Condition, other.Condition) && Equals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ObservationStatement) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Condition != null ? Condition.GetHashCode() : 0) * 397) ^ (Action != null ? Action.GetHashCode() : 0);
            }
        }
    }
}