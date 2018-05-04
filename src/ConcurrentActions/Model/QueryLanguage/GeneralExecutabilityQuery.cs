namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a single general executability query.
    /// The target of the query is to answer whether the supplied <see cref="Model.Program"/> is always executable
    /// in all models of the domain.
    /// </summary>
    public class GeneralExecutabilityQuery
    {
        /// <summary>
        /// The <see cref="Model.Program"/> whose executability should be checked.
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralExecutabilityQuery"/> class.
        /// </summary>
        /// <param name="program">The <see cref="Model.Program"/> whose executability should be checked.</param>
        public GeneralExecutabilityQuery(Program program)
        {
            Program = program;
        }

        public override string ToString()
        {
            return $"executable always {Program}";
        }

        protected bool Equals(GeneralExecutabilityQuery other)
        {
            return Equals(Program, other.Program);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GeneralExecutabilityQuery) obj);
        }

        public override int GetHashCode()
        {
            return (Program != null ? Program.GetHashCode() : 0);
        }
    }
}