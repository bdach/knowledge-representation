namespace Model.Forms
{
    /// <inheritdoc />
    /// <summary>
    /// Class representing a logical implication formula.
    /// </summary>
    public class Implication : IFormula
    {
        /// <summary>
        /// The antecedent (premise) of the implication.
        /// </summary>
        public IFormula Antecedent { get; set; }

        /// <summary>
        /// The consequent of the implication.
        /// </summary>
        public IFormula Consequent { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Implication"/> instance.
        /// </summary>
        /// <param name="antecedent">An <see cref="IFormula"/> instance representing the antecedent of the implication.</param>
        /// <param name="consequent">An <see cref="IFormula"/> instance representing the consequent of the implication.</param>
        public Implication(IFormula antecedent, IFormula consequent)
        {
            Antecedent = antecedent;
            Consequent = consequent;
        }

        /// <inheritdoc />
        public bool Evaluate(IState state)
        {
            return !Antecedent.Evaluate(state) || Consequent.Evaluate(state);
        }

        public IFormula Accept(IFormulaVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"({Antecedent} \u2192 {Consequent})";
        }

        protected bool Equals(Implication other)
        {
            return Equals(Antecedent, other.Antecedent) && Equals(Consequent, other.Consequent);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Implication) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Antecedent != null ? Antecedent.GetHashCode() : 0) * 397) ^ (Consequent != null ? Consequent.GetHashCode() : 0);
            }
        }
    }
}