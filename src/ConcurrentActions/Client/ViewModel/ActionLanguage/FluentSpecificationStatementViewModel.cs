﻿using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="FluentSpecificationStatementView"/> which represents a fluent specification statement in the scenario.
    /// </summary>
    public class FluentSpecificationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentSpecificationStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "noninertial";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "FluentSpecificationStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a fluent.
        /// </summary>
        public IViewModelFor<Model.Fluent> Fluent { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new fluent.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent { get; protected set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="FluentSpecificationStatementViewModel"/> instance.
        /// </summary>
        public FluentSpecificationStatementViewModel()
        {
            // TODO: display error?
            AddAction = ReactiveCommand.Create<ActionViewModel>(action => {});

            AddFluent = ReactiveCommand.Create<LiteralViewModel>(
                fluent => Fluent = fluent,
                this.WhenAnyValue(v => v.Fluent.IsFocused),
                RxApp.MainThreadScheduler // WARNING: Do not remove this, lest you get threading errors.
            );

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new FluentSpecificationStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="FluentSpecificationStatement"/> model represented by given view model.</returns>
        public FluentSpecificationStatement ToModel()
        {
            var fluent = Fluent?.ToModel();

            if (fluent == null)
                throw new MemberNotDefinedException("Fluent in a fluent specification statement is not defined");

            return new FluentSpecificationStatement(fluent);
        }
    }
}