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
    /// View model for <see cref="ObservationStatementView"/> which represents an observation statement in the scenario.
    /// </summary>
    public class ObservationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ObservationStatement>, IGalleryItem
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
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// Command adding a new fluent.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent { get; protected set; }

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a condition.
        /// </summary>
        public IFormulaViewModel Condition { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

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

        public ObservationStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel>(
                action => Action = action,
                this.WhenAnyValue(v => v.Action.IsFocused),
                RxApp.MainThreadScheduler // WARNING: Do not remove this, lest you get threading errors.
            );

            AddFluent = ReactiveCommand.Create<LiteralViewModel>(fluent => { });

            AddFormula = ReactiveCommand.Create<IFormulaViewModel>(
                InsertFormula,
                this.WhenAnyValue(v => v.Condition.IsFocused),
                RxApp.MainThreadScheduler
            );
        }

        private void InsertFormula(IFormulaViewModel formula)
        {
            Condition = formula.Accept(Condition);
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ObservationStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="ObservationStatement"/> model represented by given view model.</returns>
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