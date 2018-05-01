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
    /// View model for <see cref="ConditionalImpossibilityStatementView"/> which represents a conditional impossibility statement in the scenario.
    /// </summary>
    public class ConditionalImpossibilityStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<EffectStatement>, IGalleryItem
    {
        /// <summary>
        /// First keyword describing the clause.
        /// </summary>
        public string LabelLeft => "impossible";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "if";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "EffectStatement";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a precondition.
        /// </summary>
        public IFormulaViewModel Precondition { get; set; } = new PlaceholderViewModel();

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
        /// Initializes a new <see cref="ConditionalImpossibilityStatementViewModel"/> instance.
        /// </summary>
        public ConditionalImpossibilityStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(
                action => action,
                this.WhenAnyValue(vm => vm.Action.IsFocused)
            );
            AddAction.BindTo(this, vm => vm.Action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(fluent => fluent);
            
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .CombineLatest(this.WhenAnyValue(vm => vm.Precondition.IsFocused), (vm, focused) => focused ? null : vm)
                .Where(vm => vm != null)
                .InvokeCommand(this, vm => vm.Precondition.AddFormula);
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            if (Precondition.IsFocused)
            {
                Precondition = formula.Accept(Precondition);
            }
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ConditionalImpossibilityStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="EffectStatement"/> model represented by given view model.</returns>
        public EffectStatement ToModel()
        {
            var action = Action?.ToModel();
            var precondition = Precondition?.ToModel();

            if (Action == null)
                throw new MemberNotDefinedException("Action in a conditional impossibility statement is not defined");
            if (Precondition == null)
                throw new MemberNotDefinedException("Precondtition in a conditional impossibility statement is not defined");

            return EffectStatement.Impossible(action, precondition);
        }
    }
}