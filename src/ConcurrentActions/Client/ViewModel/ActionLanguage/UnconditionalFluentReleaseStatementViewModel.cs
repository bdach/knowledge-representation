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
    /// View model for <see cref="UnconditionalFluentReleaseStatementView"/> which represents an unconditional fluent release statement in the scenario.
    /// </summary>
    public class UnconditionalFluentReleaseStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentReleaseStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public static string Label => "releases";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {Label} [ ]";

        /// <summary>
        /// The <see cref="ActionViewModel"/> instance.
        /// </summary>
        public ActionViewModel Action { get; set; }

        /// <summary>
        /// The <see cref="LiteralViewModel"/> instance holding the fluent.
        /// </summary>
        public LiteralViewModel Literal { get; set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="UnconditionalFluentReleaseStatementViewModel"/> instance.
        /// </summary>
        public UnconditionalFluentReleaseStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotImplementedException());

            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="FluentReleaseStatement"/> model represented by given view model.</returns>
        public FluentReleaseStatement ToModel()
        {
            if (Action == null)
                throw new MemberNotDefinedException("Action in a conditional fluent release statement is not defined");
            if (Literal?.Fluent == null)
                throw new MemberNotDefinedException("Fluent in a conditional fluent release statement is not defined");

            return new FluentReleaseStatement(Action.ToModel(), Literal.Fluent, Constant.Truth);
        }
    }
}