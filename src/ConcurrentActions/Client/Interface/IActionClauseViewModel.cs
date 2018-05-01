﻿using System.Reactive;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
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
        ReactiveCommand<ActionViewModel, ActionViewModel> AddAction { get; }

        /// <summary>
        /// Adds a new <see cref="Model.Fluent"/> to edited clause.
        /// This is intended for statements which take a single fluent.
        /// </summary>
        ReactiveCommand<LiteralViewModel, LiteralViewModel> AddFluent { get; }
        
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

        /// <summary>
        /// Property determining whether the current node in the view model tree is focused.
        /// </summary>
        bool IsFocused { get; set; }

        /// <summary>
        /// Command triggered by delete key used to delete currently selected clause element.
        /// </summary>
        ReactiveCommand<Unit, Unit> DeleteFocused { get; }
    }
}