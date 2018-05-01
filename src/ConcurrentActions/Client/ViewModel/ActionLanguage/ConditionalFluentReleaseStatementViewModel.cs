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
    /// View model for <see cref="ConditionalFluentReleaseStatementView"/> which represents a conditional fluent release statement in the scenario.
    /// </summary>
    public class ConditionalFluentReleaseStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentReleaseStatement>, IGalleryItem
    {
        /// <summary>
        /// First keyword describing the clause.
        /// </summary>
        public string LabelLeft => "releases";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "if";
        /// <inheritdoc />
        public string ClauseTypeNameKey => "FluentReleaseStatement";

        /// <summary>
        /// Name of the query displayed in dropdown menu.
        /// </summary>
        public string DisplayName => $"[ ] {LabelLeft} [ ] {LabelRight} [ ]";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning an action.
        /// </summary>
        public IViewModelFor<Model.Action> Action { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a fluent.
        /// </summary>
        public IViewModelFor<Model.Fluent> Fluent { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a precondition.
        /// </summary>
        public IViewModelFor<IFormula> Precondition { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// Command adding a new action.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction { get; protected set; }

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IViewModelFor<IFormula>, Unit> AddFormula { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ConditionalFluentReleaseStatementViewModel"/> instance.
        /// </summary>
        public ConditionalFluentReleaseStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel>(
                action => Action = action,
                this.WhenAnyValue(v => v.Action.IsFocused),
                RxApp.MainThreadScheduler // WARNING: Do not remove this, lest you get threading errors.
            );

            AddFormula = ReactiveCommand
                .Create<IViewModelFor<IFormula>>(formulaViewModel =>
                    throw new NotImplementedException());
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new ConditionalFluentReleaseStatementViewModel();
        }

        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="FluentReleaseStatement"/> model represented by given view model.</returns>
        public FluentReleaseStatement ToModel()
        {
            var action = Action?.ToModel();
            var fluent = Fluent?.ToModel();
            var precondition = Precondition?.ToModel();

            if (action == null)
                throw new MemberNotDefinedException("Action in a conditional fluent release statement is not defined");
            if (fluent == null)
                throw new MemberNotDefinedException("Fluent in a conditional fluent release statement is not defined");
            if (precondition == null)
                throw new MemberNotDefinedException("Precondtition in a conditional fluent release statement is not defined");

            return new FluentReleaseStatement(action, fluent, precondition);
        }
    }
}