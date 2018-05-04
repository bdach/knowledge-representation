using System.Reactive;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Common interface for all viewmodels of elements used in the domain editor.
    /// </summary>
    public interface IEditorViewModel
    {
        /// <summary>
        /// Property determining whether the current node in the view model tree is focused.
        /// </summary>
        bool IsFocused { get; set; }

        /// <summary>
        /// Command triggered by delete key used to delete currently selected clause element.
        /// </summary>
        ReactiveCommand<Unit, Unit> DeleteFocused { get; }

        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; }
    }
}