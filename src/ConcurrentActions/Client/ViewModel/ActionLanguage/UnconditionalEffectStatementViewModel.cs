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
    /// View model for <see cref="UnconditionalEffectStatementView"/> which represents an unconditional effect statement in the scenario.
    /// </summary>
    public class UnconditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "causes";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "EffectStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a postcondition.
        /// </summary>
        public IFormulaViewModel Postcondition { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="UnconditionalEffectStatementViewModel"/> instance.
        /// </summary>
        public UnconditionalEffectStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(
                action => action,
                this.WhenAnyValue(v => v.Action.IsFocused)
            );
            AddAction.BindTo(this, vm => vm.Action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Postcondition.AddFormula);
            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => Action.IsFocused).Subscribe(_ => Action = new PlaceholderViewModel());
            DeleteFocused.Where(_ => Postcondition.IsFocused).Subscribe(_ => Postcondition = new PlaceholderViewModel());

            DeleteFocused.Where(_ => !(Action.IsFocused || Postcondition.IsFocused))
                .InvokeCommand(this, vm => vm.Action.DeleteFocused);
            DeleteFocused.Where(_ => !(Action.IsFocused || Postcondition.IsFocused))
                .InvokeCommand(this, vm => vm.Postcondition.DeleteFocused);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Postcondition.IsFocused)
            {
                Postcondition = formula.Accept(Postcondition);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new UnconditionalEffectStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        public EffectStatement ToModel()
        {
            var action = Action?.ToModel();
            var postcondition = Postcondition?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("Action in an unconditional effect statement is not defined");
            if (postcondition == null)
                throw new MemberNotDefinedException("Postcondition in an unconditional effect statement is not defined");

            return new EffectStatement(action, postcondition);
        }
    }
}