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
    /// View model for <see cref="InitialValueStatementView"/> which represents an initial value statement in the scenario.
    /// </summary>
    public class InitialValueStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<InitialValueStatement>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "initially";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "InitialValueStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an initial condition.
        /// </summary>
        public IViewModelFor<IFormula> InitialCondition { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="InitialValueStatementViewModel"/> instance.
        /// </summary>
        public InitialValueStatementViewModel()
        {
            AddAction = ReactiveCommand
                .Create<ActionViewModel>(actionViewModel =>
                    throw new NotApplicableException("Initial value statement does not support adding actions"));

            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
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