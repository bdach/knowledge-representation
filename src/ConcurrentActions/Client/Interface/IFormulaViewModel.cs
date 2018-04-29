using System.Reactive;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every logical expression.
    /// </summary>
    public interface IFormulaViewModel : IViewModelFor<IFormula>
    {
        // TODO: try to think of a clever way to have this called once - just for the currently selected view model (instead of all from the clause tree until we reach selected control)

        /// <summary>
        /// Adds a new <see cref="IFormulaViewModel"/> to edited formula.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; }
    }
}