using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single fluent release statement.
    /// Fluent release statements are used to model nondeterministic results of actions.
    /// </summary>
    public class FluentReleaseStatement
    {
        /// <summary>
        /// The <see cref="Model.Action"/> associated with the fluent release.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// The <see cref="Model.Fluent"/> to be released.
        /// </summary>
        public Fluent Fluent { get; set; }

        /// <summary>
        /// The <see cref="IFormula"/> representing the precondition of the fluent release.
        /// </summary>
        public IFormula Precondition { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public FluentReleaseStatement() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentReleaseStatement"/> class.
        /// </summary>
        /// <param name="action">The <see cref="Model.Action"/> associated with the fluent release.</param>
        /// <param name="fluent">The <see cref="Model.Fluent"/> to be released.</param>
        /// <param name="precondition">The <see cref="IFormula"/> representing the precondition of the fluent release.</param>
        public FluentReleaseStatement(Action action, Fluent fluent, IFormula precondition)
        {
            Action = action;
            Fluent = fluent;
            Precondition = precondition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentReleaseStatement"/> class.
        /// This constructor assumes that the fluent release is unconditional (the precondition is always true).
        /// </summary>
        /// <param name="action">The <see cref="Model.Action"/> associated with the fluent release.</param>
        /// <param name="fluent">The <see cref="Model.Fluent"/> to be released.</param>
        public FluentReleaseStatement(Action action, Fluent fluent) : this(action, fluent, Constant.Truth)
        {

        }

        public override string ToString()
        {
            var prefix = $"{Action} releases {Fluent}";
            var suffix = Precondition == Constant.Truth ? "" : $"if {Precondition}";
            return string.Join(" ", prefix, suffix);
        }
    }
}