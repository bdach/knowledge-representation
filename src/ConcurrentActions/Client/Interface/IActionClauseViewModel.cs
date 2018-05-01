using System.Reactive;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Action Language clause.
    /// </summary>
    public interface IActionClauseViewModel : IClauseViewModel
    {
        /// <summary>
        /// Adds a new <see cref="ActionViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<ActionViewModel, Unit> AddAction { get; }

        /// <summary>
        /// Adds a new <see cref="Model.Fluent"/> to edited clause.
        /// This is intended for statements which take a single fluent.
        /// </summary>
        ReactiveCommand<LiteralViewModel, Unit> AddFluent { get; }
        
        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; }

        /// <summary>
        /// Creates a new empty instance of a given type of action statement.
        /// Implementation of the prototype pattern.
        /// </summary>
        /// <returns>New empty instance of a <see cref="IActionClauseViewModel"/>.</returns>
        IActionClauseViewModel NewInstance();
    }
}