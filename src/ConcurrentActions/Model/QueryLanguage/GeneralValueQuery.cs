using Model.Forms;

namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a single general value query.
    /// The target of the query is to answer whether the execution of the supplied <see cref="Model.Program"/>
    /// always leads to the satisfaction of the supplied <see cref="IFormula"/>.
    /// </summary>
    public class GeneralValueQuery
    {
        /// <summary>
        /// The target <see cref="IFormula"/> to be executed.
        /// </summary>
        public IFormula Target { get; set; }

        /// <summary>
        /// The <see cref="Model.Program"/> to execute.
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public GeneralValueQuery() { }

        /// <summary>
        /// Initializes an instance of the <see cref="GeneralValueQuery"/> class.
        /// </summary>
        /// <param name="target">The target <see cref="IFormula"/> to be executed.</param>
        /// <param name="program">The <see cref="Model.Program"/> to execute.</param>
        public GeneralValueQuery(IFormula target, Program program)
        {
            Target = target;
            Program = program;
        }

        public override string ToString()
        {
            return $"necessary {Target} after {Program}";
        }
    }
}