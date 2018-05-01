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
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="ConditionalEffectStatementView"/> which represents a constraint statement in the scenario.
    /// </summary>
    public class ConstraintStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ConstraintStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "always";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ConstraintStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a constraint.
        /// </summary>
        public IFormulaViewModel Constraint { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="ConstraintStatementViewModel"/> instance.
        /// </summary>
        public ConstraintStatementViewModel()
        {
            // TODO: display error?
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Constraint.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => Constraint.IsFocused).Subscribe(_ => Constraint = new PlaceholderViewModel());
            DeleteFocused.Where(_ => !Constraint.IsFocused).InvokeCommand(this, vm => vm.Constraint.DeleteFocused);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Constraint.IsFocused)
            {
                Constraint = formula.Accept(Constraint);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ConstraintStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="ConstraintStatement"/> model represented by given view model.</returns>
        public ConstraintStatement ToModel()
        {
            var constraint = Constraint?.ToModel();

            if (constraint == null)
                throw new MemberNotDefinedException("Constraint condition in a constraint statement is not defined");

            return new ConstraintStatement(constraint);
        }
    }
}