using Model.Forms;

namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a single general value query.
    /// The target of the query is to answer whether the execution of the supplied <see cref="Model.Program"/>
    /// sometimes leads to the satisfaction of the supplied <see cref="IFormula"/>.
    /// </summary>
    public class ExistentialValueQuery
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
        public ExistentialValueQuery() { }

        /// <summary>
        /// Initializes an instance of the <see cref="ExistentialValueQuery"/> class.
        /// </summary>
        /// <param name="target">The target <see cref="IFormula"/> to be executed.</param>
        /// <param name="program">The <see cref="Model.Program"/> to execute.</param>
        public ExistentialValueQuery(IFormula target, Program program)
        {
            Target = target;
            Program = program;
        }

        public override string ToString()
        {
            return $"possibly {Target} after {Program}";
        }

        protected bool Equals(ExistentialValueQuery other)
        {
            return Equals(Target, other.Target) && Equals(Program, other.Program);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExistentialValueQuery) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Target != null ? Target.GetHashCode() : 0) * 397) ^ (Program != null ? Program.GetHashCode() : 0);
            }
        }
    }
}