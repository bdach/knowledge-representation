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
        /// Initializes a new <see cref="ConditionalFluentReleaseStatementViewModel"/> instance.
        /// </summary>
        public ConditionalFluentReleaseStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(
                action => action,
                this.WhenAnyValue(v => v.Action.IsFocused)
            );
            AddAction.BindTo(this, vm => vm.Action);

            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(
                fluent => fluent,
                this.WhenAnyValue(v => v.Fluent.IsFocused)
            );
            AddFluent.BindTo(this, vm => vm.Fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);

            this.WhenAnyObservable(vm => vm.AddFormula)
                .CombineLatest(this.WhenAnyValue(vm => vm.IsFocused), (vm, focused) => focused ? null : vm)
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