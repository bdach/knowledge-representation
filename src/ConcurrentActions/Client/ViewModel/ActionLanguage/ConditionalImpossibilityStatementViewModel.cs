using System;
using System.Reactive;
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
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent { get; protected set; }

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ConditionalImpossibilityStatementViewModel"/> instance.
        /// </summary>
        public ConditionalImpossibilityStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel>(
                action => Action = action,
                this.WhenAnyValue(vm => vm.Action.IsFocused),
                RxApp.MainThreadScheduler // WARNING: Do not remove this, lest you get threading errors.
            );

            AddFluent = ReactiveCommand.Create<LiteralViewModel>(fluent => { });

            AddFormula = ReactiveCommand.Create<IFormulaViewModel>(
                InsertFormula,
                this.WhenAnyValue(v => v.Precondition.IsFocused),
                RxApp.MainThreadScheduler
            );
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            Precondition = formula.Accept(Precondition);
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