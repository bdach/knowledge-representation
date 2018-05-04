using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
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
    /// View model for <see cref="UnconditionalImpossibilityStatementView"/> which represents an unconditional impossibility statement in the scenario.
    /// </summary>
    public class UnconditionalImpossibilityStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "impossible";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "EffectStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="UnconditionalImpossibilityStatementViewModel"/> instance.
        /// </summary>
        public UnconditionalImpossibilityStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(
                action => action,
                this.WhenAnyValue(v => v.Action.IsFocused)
            );
            AddAction.BindTo(this, vm => vm.Action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => IsFocused || Action.IsFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddFormulaError"));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Action.IsFocused)
                .Subscribe(_ => Action = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Action.IsFocused)
                .InvokeCommand(this, vm => vm.Action.DeleteFocused);
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new UnconditionalImpossibilityStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        public EffectStatement ToModel()
        {
            var action = Action?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("Action in an unconditional impossibility statement is not defined");

            return EffectStatement.Impossible(action, Constant.Truth);
        }
    }
}