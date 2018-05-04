﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="UnconditionalFluentReleaseStatementView"/> which represents an unconditional fluent release statement in the scenario.
    /// </summary>
    public class UnconditionalFluentReleaseStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentReleaseStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "releases";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "FluentReleaseStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a literal.
        /// </summary>
        public IViewModelFor<Model.Fluent> Fluent { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new fluent.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, LiteralViewModel> AddFluent { get; protected set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, ActionViewModel> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="UnconditionalFluentReleaseStatementViewModel"/> instance.
        /// </summary>
        public UnconditionalFluentReleaseStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(
                action => action,
                this.WhenAnyValue(v => v.Action.IsFocused)
            );
            AddAction.BindTo(this, vm => vm.Action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(
                fluent => fluent,
                this.WhenAnyValue(v => v.Fluent.IsFocused)
            );
            AddFluent.BindTo(this, vm => vm.Fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Action.IsFocused)
                .Subscribe(_ => Action = new PlaceholderViewModel());
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Fluent.IsFocused)
                .Subscribe(_ => Fluent = new PlaceholderViewModel());
            
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Action.IsFocused || Fluent.IsFocused))
                .InvokeCommand(this, vm => vm.Action.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Action.IsFocused || Fluent.IsFocused))
                .InvokeCommand(this, vm => vm.Fluent.DeleteFocused);
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new UnconditionalFluentReleaseStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="FluentReleaseStatement"/> model represented by given view model.</returns>
        public FluentReleaseStatement ToModel()
        {
            var action = Action?.ToModel();
            var fluent = Fluent?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("Action in a conditional fluent release statement is not defined");
            if (fluent == null)
                throw new MemberNotDefinedException("Fluent in a conditional fluent release statement is not defined");

            return new FluentReleaseStatement(action, fluent);
        }
    }
}