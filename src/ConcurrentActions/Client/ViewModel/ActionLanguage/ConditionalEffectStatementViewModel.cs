using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using ControlzEx.Standard;
using Model.ActionLanguage;
using Model.Forms;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="ConditionalEffectStatementView"/> which represents a conditional effect statement in the scenario.
    /// </summary>
    public class ConditionalEffectStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
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
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight} [ ]";

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

        /// <summary>
        /// Initializes a new <see cref="ConditionalEffectStatementViewModel"/> instance.
        /// </summary>
        public ConditionalEffectStatementViewModel()
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
                .CombineLatest(this.WhenAnyValue(vm => vm.IsFocused), (vm, focused) => focused ? null : vm)
                .Where(vm => vm != null)
                .InvokeCommand(this, vm => vm.Precondition.AddFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .CombineLatest(this.WhenAnyValue(vm => vm.IsFocused), (vm, focused) => focused ? null : vm)
                .Where(vm => vm != null)
                .InvokeCommand(this, vm => vm.Postcondition.AddFormula);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
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

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        public EffectStatement ToModel()
        {
            var action = Action?.ToModel();
            var precondition = Precondition?.ToModel();
            var postcondition = Postcondition?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("Action in a conditional effect statement is not defined");
            if (precondition == null)
                throw new MemberNotDefinedException("Precondtition in a conditional effect statement is not defined");
            if (postcondition == null)
                throw new MemberNotDefinedException("Postcondition in a conditional effect statement is not defined");

            return new EffectStatement(action, precondition, postcondition);
        }
    }
}