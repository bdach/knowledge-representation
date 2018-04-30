using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.ActionLanguage;
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

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a condition.
        /// </summary>
        public IViewModelFor<IFormula> Condition { get; set; } = new PlaceholderViewModel();

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
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        public ObservationStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotImplementedException());

            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
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