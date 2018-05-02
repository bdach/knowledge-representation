using System.Reactive;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Query Language clause.
    /// </summary>
    public interface IQueryClauseViewModel : IClauseViewModel
    {
        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; }

        /// <summary>
        /// Adds a new <see cref="ProgramViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; }

        /// <summary>
        /// Creates a new empty instance of a given type of query.
        /// Implementation of the prototype pattern.
        /// </summary>
        /// <returns>New empty instance of a <see cref="IQueryClauseViewModel"/>.</returns>
        IQueryClauseViewModel NewInstance();
    }
}