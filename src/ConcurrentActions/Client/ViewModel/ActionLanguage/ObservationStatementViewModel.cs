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
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="ObservationStatementView"/> which represents an observation statement in the scenario.
    /// </summary>
    public class ObservationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ObservationStatement>
    {
        /// <summary>
        /// First keyword describing the clause.
        /// </summary>
        public string LabelLeft => "observable";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "after";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ObservationStatement";

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

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused || Condition.AnyChildFocused || Action.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        public ObservationStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            AddAction.Where(_ => Action.IsFocused)
                .BindTo(this, vm => vm.Action);
            this.WhenAnyObservable(vm => vm.AddAction)
                .Where(_ => Condition.AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            // TODO: wrong user choice COULD be handled here somehow, but *I'm* NOT doing it
            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => Action.IsFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddFormulaError"));
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Condition.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Condition.IsFocused)
                .Subscribe(_ => Condition = new PlaceholderViewModel());
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Action.IsFocused)
                .Subscribe(_ => Action = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Condition.IsFocused || Action.IsFocused))
                .InvokeCommand(this, vm => vm.Condition.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Condition.IsFocused || Action.IsFocused))
                .InvokeCommand(this, vm => vm.Action.DeleteFocused);
        }

        /// <summary>
        /// Function used to handle formula insertion based on window focus.
        /// </summary>
        /// <param name="formula">The <see cref="IFormulaViewModel"/> instance to be inserted.</param>
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
            return new ObservationStatementViewModel();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="ObservationStatement"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public ObservationStatement ToModel()
        {
            var condition = Condition?.ToModel();
            var action = Action?.ToModel();

            if (condition == null)
                throw new MemberNotDefinedException("Condition in an observation statement is not defined");
            if (action == null)
                throw new MemberNotDefinedException("Action in an observation statement is not defined");

            return new ObservationStatement(condition, action);
        }
    }
}