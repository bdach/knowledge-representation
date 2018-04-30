using System;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model.Forms;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="AccessibilityQueryView"/> which represents an accessibility query in the scenario.
    /// </summary>
    public class AccessibilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<AccessibilityQuery>, IGalleryItem
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "accessible";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "AccessibilityQuery";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"{Label} [ ]";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a target formula.
        /// </summary>
        public IViewModelFor<IFormula> Target { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<ProgramViewModel, Unit> AddProgram { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="AccessibilityQueryViewModel"/> instance.
        /// </summary>
        public AccessibilityQueryViewModel()
        {
            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());

            AddProgram = ReactiveCommand
                .Create<ProgramViewModel>(programViewModel =>
                    throw new NotApplicableException("Accessibility query does not support adding programs"));
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