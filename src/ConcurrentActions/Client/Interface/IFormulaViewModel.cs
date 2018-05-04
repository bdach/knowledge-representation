using Model.Forms;
using ReactiveUI;

namespace Client.Interface
{
    /// <inheritdoc />
    /// <summary>
    /// Common interface for all formula view models.
    /// </summary>
    public interface IFormulaViewModel : IViewModelFor<IFormula>
    {
        /// <summary>
        /// Takes an existing formula in the editor and either replaces it with another one, or adds it as a child.
        /// </summary>
        /// <param name="existingFormula">Existing instance of <see cref="IFormulaViewModel"/>.</param>
        /// <returns>New instance of <see cref="IFormulaViewModel"/>, to be substituted in place of the old one.</returns>
        IFormulaViewModel Accept(IFormulaViewModel existingFormula);

        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; }
    }
}