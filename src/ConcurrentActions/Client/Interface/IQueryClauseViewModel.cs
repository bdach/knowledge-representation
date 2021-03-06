﻿using System.Reactive;
using Client.ViewModel.Terminal;
using DynamicSystem;
using ReactiveUI;

namespace Client.Interface
{
    /// <summary>
    /// Base interface for every Query Language clause.
    /// </summary>
    public interface IQueryClauseViewModel : IClauseViewModel
    {
        /// <summary>
        /// Adds a new empty <see cref="CompoundActionViewModel"/> to edited clause.
        /// </summary>
        ReactiveCommand<Unit, Unit> AddEmptyCompoundAction { get; }

        /// <summary>
        /// Forwards an <see cref="ActionViewModel"/> to the program's compound actions for addition.
        /// </summary>
        ReactiveCommand<ActionViewModel, ActionViewModel> AddAtomicAction { get; }

        /// <summary>
        /// Creates a new empty instance of a given type of query.
        /// Implementation of the prototype pattern.
        /// </summary>
        /// <returns>New empty instance of a <see cref="IQueryClauseViewModel"/>.</returns>
        IQueryClauseViewModel NewInstance();

        /// <summary>
        /// Stores the result of the query, if available.
        /// </summary>
        bool? Result { get; set; }

        /// <summary>
        /// Stores the result of the given query in the <see cref="Result"/> property.
        /// </summary>
        /// <param name="results">Instance of <see cref="QueryResolution"/> containing query results.</param>
        void AcceptResult(QueryResolution results);
    }
}