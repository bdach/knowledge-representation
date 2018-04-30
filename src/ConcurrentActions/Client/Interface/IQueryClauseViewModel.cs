using System.Reactive;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Query Language clause.
    /// </summary>
    public interface IQueryClauseViewModel
    {
        /// <summary>
        /// Adds a new <see cref="IViewModelFor{IFormula}"/> to edited clause.
        /// </summary>
        ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; }

        /// <summary>
        /// Adds a new <see cref="ProgramViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; }
    }
}