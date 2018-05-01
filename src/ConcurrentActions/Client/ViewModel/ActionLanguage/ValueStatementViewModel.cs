using System;
using System.Reactive;
using System.Reactive.Linq;
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
    /// View model for <see cref="ValueStatementView"/> which represents a value statement in the scenario.
    /// </summary>
    public class ValueStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ValueStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "after";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {Label} [ ]";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ValueStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a condition.
        /// </summary>
        public IFormulaViewModel Condition { get; set; } = new PlaceholderViewModel();

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

        /// <summary>
        /// Initializes a new <see cref="ValueStatementViewModel"/> instance.
        /// </summary>
        public ValueStatementViewModel()
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
                .CombineLatest(this.WhenAnyValue(vm => vm.Condition.IsFocused), (vm, focused) => focused ? null : vm)
                .Where(vm => vm != null)
                .InvokeCommand(this, vm => vm.Condition.AddFormula);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Condition.IsFocused)
            {
                Condition = formula.Accept(Condition);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ValueStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="ValueStatement"/> model represented by given view model.</returns>
        public ValueStatement ToModel()
        {
            var condition = Condition?.ToModel();
            var action = Action?.ToModel();

            if (condition == null)
                throw new MemberNotDefinedException("Condition in a value statement is not defined");
            if (action == null)
                throw new MemberNotDefinedException("Action in a value statement is not defined");

            return new ValueStatement(condition, action);
        }
    }
}