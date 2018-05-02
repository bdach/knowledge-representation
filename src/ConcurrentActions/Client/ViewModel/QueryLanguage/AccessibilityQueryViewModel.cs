using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="AccessibilityQueryView"/> which represents an accessibility query in the scenario.
    /// </summary>
    public class AccessibilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<AccessibilityQuery>
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "accessible";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "AccessibilityQuery";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a target formula.
        /// </summary>
        public IFormulaViewModel Target { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="AccessibilityQueryViewModel"/> instance.
        /// </summary>
        public AccessibilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IFormulaViewModel>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotApplicableException("Accessibility query does not support adding programs"));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new AccessibilityQueryViewModel();
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="AccessibilityQuery"/> model represented by given view model.</returns>
        public AccessibilityQuery ToModel()
        {
            var target = Target?.ToModel();

            if (target == null)
                throw new MemberNotDefinedException("Target in an accessibility query is not defined");

            return new AccessibilityQuery(target);
        }
    }
}