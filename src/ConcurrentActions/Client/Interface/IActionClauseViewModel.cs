using System.Reactive;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Action Language clause.
    /// </summary>
    public interface IActionClauseViewModel
    {
        /// <summary>
        /// Adds a new <see cref="ActionViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<ActionViewModel, Unit> AddAction { get; }
        
        /// <summary>
        /// Adds a new <see cref="IFormulaViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; }
    }
}