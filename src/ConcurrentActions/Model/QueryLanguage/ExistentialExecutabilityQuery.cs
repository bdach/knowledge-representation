namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a single existential executability query.
    /// The target of the query is to answer whether the supplied <see cref="Model.Program"/> is ever executable
    /// in all models of the domain.
    /// </summary>
    public class ExistentialExecutabilityQuery
    {
        /// <summary>
        /// The <see cref="Model.Program"/> whose executability should be checked.
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistentialExecutabilityQuery"/> class.
        /// </summary>
        /// <param name="program">The <see cref="Model.Program"/> whose executability should be checked.</param>
        public ExistentialExecutabilityQuery(Program program)
        {
            Program = program;
        }

        public override string ToString()
        {
            return $"executable sometimes {Program}";
        }

        protected bool Equals(ExistentialExecutabilityQuery other)
        {
            return Equals(Program, other.Program);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExistentialExecutabilityQuery) obj);
        }

        public override int GetHashCode()
        {
            return (Program != null ? Program.GetHashCode() : 0);
        }
    }
}