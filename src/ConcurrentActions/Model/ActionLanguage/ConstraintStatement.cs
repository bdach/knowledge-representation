using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single constraint statement in the action domain.
    /// </summary>
    public class ConstraintStatement
    {
        /// <summary>
        /// The <see cref="IFormula"/> instance describing the constraint all states in the model must adhere to.
        /// </summary>
        public IFormula Constraint { get; set; }

        /// <summary>
        /// Empty construction required by serialization.
        /// </summary>
        public ConstraintStatement() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintStatement"/>, using the supplied constraint.
        /// </summary>
        /// <param name="constraint">An instance of <see cref="IFormula"/> describing the constraint all states in the model must adhere to.</param>
        public ConstraintStatement(IFormula constraint)
        {
            Constraint = constraint;
        }

        public override string ToString()
        {
            return $"always {Constraint}";
        }
    }
}