using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single initial value statement in the action domain.
    /// </summary>
    public class InitialValueStatement
    {
        /// <summary>
        /// The <see cref="IFormula"/> instance representing the initial value condition.
        /// </summary>
        public IFormula InitialCondition { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public InitialValueStatement() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InitialValueStatement"/> class, using the supplied condition.
        /// </summary>
        /// <param name="initialCondition">An instance of <see cref="IFormula"/> representing the initial value condition.</param>
        public InitialValueStatement(IFormula initialCondition)
        {
            InitialCondition = initialCondition;
        }

        public override string ToString()
        {
            return $"initially {InitialCondition}";
        }
    }
}