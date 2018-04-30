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
    /// View model for <see cref="ConditionalEffectStatementView"/> which represents a constraint statement in the scenario.
    /// </summary>
    public class ConstraintStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<ConstraintStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "always";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a constraint.
        /// </summary>
        public IViewModelFor<IFormula> Constraint { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ConstraintStatementViewModel"/> instance.
        /// </summary>
        public ConstraintStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Constraint statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
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