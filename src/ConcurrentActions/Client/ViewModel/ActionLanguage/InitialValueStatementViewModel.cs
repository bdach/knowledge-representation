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
    /// View model for <see cref="InitialValueStatementView"/> which represents an initial value statement in the scenario.
    /// </summary>
    public class InitialValueStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<InitialValueStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "initially";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "InitialValueStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an initial condition.
        /// </summary>
        public IFormulaViewModel InitialCondition { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="InitialValueStatementViewModel"/> instance.
        /// </summary>
        public InitialValueStatementViewModel()
        {
            // TODO: display error?
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel> (action => action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.InitialCondition.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => InitialCondition.IsFocused).Subscribe(_ => InitialCondition = new PlaceholderViewModel());
            DeleteFocused.Where(_ => !InitialCondition.IsFocused).InvokeCommand(this, vm => vm.InitialCondition.DeleteFocused);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (InitialCondition.IsFocused)
            {
                InitialCondition = formula.Accept(InitialCondition);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new InitialValueStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="InitialValueStatement"/> model represented by given view model.</returns>
        public InitialValueStatement ToModel()
        {
            var initialCondition = InitialCondition?.ToModel();

            if (initialCondition == null)
                throw new MemberNotDefinedException("Initial condition in an initial value statement is not defined");

            return new InitialValueStatement(initialCondition);
        }
    }
}