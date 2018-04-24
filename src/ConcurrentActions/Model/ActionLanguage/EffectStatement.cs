using Model.Forms;

namespace Model.ActionLanguage
{
    /// <summary>
    /// Represents a single effect statement in the action domain.
    /// </summary>
    public class EffectStatement
    {
        /// <summary>
        /// The <see cref="Model.Action"/> whose effects are described by the statement.
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// The <see cref="IFormula"/> instance describing the precondition of the effect statement.
        /// </summary>
        public IFormula Precondition { get; set; }

        /// <summary>
        /// The <see cref="IFormula"/> instance describing the postcondition (the actual effect) of the effect statement.
        /// </summary>
        public IFormula Postcondition { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EffectStatement"/> class.
        /// </summary>
        /// <param name="action">The <see cref="Model.Action"/> whose effects are described by the statement.</param>
        /// <param name="precondition">The <see cref="IFormula"/> instance describing the precondition of the effect statement.</param>
        /// <param name="postcondition">The <see cref="IFormula"/> instance describing the postcondition (the actual effect) of the effect statement.</param>
        public EffectStatement(Action action, IFormula precondition, IFormula postcondition)
        {
            Action = action;
            Precondition = precondition;
            Postcondition = postcondition;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Model.ActionLanguage.EffectStatement" /> class.
        /// This constructor assumes truth as the precondition (it is assumed the effects of this statement always occur upon execution of the supplied <see cref="!:action" />).
        /// </summary>
        public EffectStatement(Action action, IFormula postcondition) : this(action, Constant.Truth, postcondition)
        {
        }

        /// <summary>
        /// Shorthand function, used to represent impossibility statements.
        /// </summary>
        /// <remarks>
        /// Impossibility statements are represented as effect statements with a constant falsity postcondition.
        /// This means the action is never actually executable, as the postcondition can never be satisfied.
        /// </remarks>
        /// <param name="action">The <see cref="Model.Action"/> whose effects are described by the statement.</param>
        /// <param name="precondition">The <see cref="IFormula"/> instance describing the precondition of the effect statement.</param>
        /// <returns></returns>
        public static EffectStatement Impossible(Action action, IFormula precondition)
        {
            return new EffectStatement(action, precondition, Constant.Falsity);
        }

        public override string ToString()
        {
            var prefix = Postcondition == Constant.Falsity
                ? $"impossible {Action}"
                : $"{Action} causes {Postcondition}";
            var suffix = Precondition == Constant.Truth ? "" : $"if {Precondition}";
            return string.Join(" ", prefix, suffix);
        }
    }
}