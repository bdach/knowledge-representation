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

        protected bool Equals(ValueStatement other)
        {
            return Equals(Condition, other.Condition) && Equals(Action, other.Action);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ValueStatement) obj);
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