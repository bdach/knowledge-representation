using System.Reactive;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Query Language clause.
    /// </summary>
    public interface IQueryClauseViewModel
    {
        /// <summary>
        /// Adds a new <see cref="IFormulaViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; }

        /// <summary>
        /// Adds a new <see cref="ProgramViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; }
    }
}