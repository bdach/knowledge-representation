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
    /// View model for <see cref="ConditionalEffectStatementView"/> which represents a conditional effect statement in the scenario.
    /// </summary>
    public class ConditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>
    {
        /// <summary>
        /// First keyword describing the clause.
        /// </summary>
        public string LabelLeft => "causes";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "if";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "EffectStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a precondition.
        /// </summary>
        public IFormulaViewModel Precondition { get; set; } = new PlaceholderViewModel();

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
        public bool AnyChildFocused => IsFocused || Action.AnyChildFocused || Precondition.AnyChildFocused || Postcondition.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ConditionalEffectStatementViewModel"/> instance.
        /// </summary>
        public ConditionalEffectStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            AddAction.Where(_ => Action.IsFocused)
                .BindTo(this, vm => vm.Action);
            this.WhenAnyObservable(vm => vm.AddAction)
                .Where(_ => Precondition.AnyChildFocused || Postcondition.AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            // TODO: wrong user choice COULD be handled here somehow, but *I'm* NOT doing it
            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Precondition.AddFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Postcondition.AddFormula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Action.IsFocused)
                .Subscribe(_ => Action = new PlaceholderViewModel());
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Precondition.IsFocused)
                .Subscribe(_ => Precondition = new PlaceholderViewModel());
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Postcondition.IsFocused)
                .Subscribe(_ => Postcondition = new PlaceholderViewModel());
            
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Action.IsFocused || Precondition.IsFocused || Postcondition.IsFocused))
                .InvokeCommand(this, vm => vm.Action.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Action.IsFocused || Precondition.IsFocused || Postcondition.IsFocused))
                .InvokeCommand(this, vm => vm.Precondition.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Action.IsFocused || Precondition.IsFocused || Postcondition.IsFocused))
                .InvokeCommand(this, vm => vm.Postcondition.DeleteFocused);
        }

        /// <summary>
        /// Function used to handle formula insertion based on window focus.
        /// </summary>
        /// <param name="formula">The <see cref="IFormulaViewModel"/> instance to be inserted.</param>
        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Action.IsFocused)
            {
                Interactions.RaiseStatusBarError("CannotAddFormulaError");
            }
            if (Precondition.IsFocused)
            {
                Precondition = formula.Accept(Precondition);
            }
            if (Postcondition.IsFocused)
            {
                Postcondition = formula.Accept(Postcondition);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ConditionalEffectStatementViewModel();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public EffectStatement ToModel()
        {
            var action = Action?.ToModel();
            var precondition = Precondition?.ToModel();
            var postcondition = Postcondition?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("ConditionalEffectStatementActionError");
            if (precondition == null)
                throw new MemberNotDefinedException("ConditionalEffectStatementPreconditionError");
            if (postcondition == null)
                throw new MemberNotDefinedException("ConditionalEffectStatementPostconditionError");

            return new EffectStatement(action, precondition, postcondition);
        }
    }
}