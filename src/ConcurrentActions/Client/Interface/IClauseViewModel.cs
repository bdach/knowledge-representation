using Client.ViewModel.Formula;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for clause view models.
    /// </summary>
    public interface IClauseViewModel : IEditorViewModel
    {
        /// <summary>
        /// The localization key for this clause type.
        /// Used for ribbon grouping.
        /// </summary>
        string ClauseTypeNameKey { get; }

        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; }
    }
}