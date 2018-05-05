using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.View.ActionLanguage;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.ActionLanguage;
using ReactiveUI;

namespace Client.ViewModel.ActionLanguage
{
    /// <summary>
    /// View model for <see cref="FluentSpecificationStatementView"/> which represents a fluent specification statement in the scenario.
    /// </summary>
    public class FluentSpecificationStatementViewModel : FodyReactiveObject, IActionClauseViewModel, IViewModelFor<FluentSpecificationStatement>
    {
        /// <summary>
        /// Keyword describing the clause.
        /// </summary>
        public string Label => "noninertial";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "FluentSpecificationStatement";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a fluent.
        /// </summary>
        public IViewModelFor<Model.Fluent> Fluent { get; set; } = new PlaceholderViewModel();

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

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused || Fluent.IsFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="FluentSpecificationStatementViewModel"/> instance.
        /// </summary>
        public FluentSpecificationStatementViewModel()
        {
            AddAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            this.WhenAnyObservable(vm => vm.AddAction)
                .Where(_ => AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            // TODO: wrong user choice COULD be handled here somehow, but *I'm* NOT doing it
            AddFluent = ReactiveCommand.Create<LiteralViewModel, LiteralViewModel>(
                fluent => fluent,
                this.WhenAnyValue(v => v.Fluent.IsFocused)
            );
            AddFluent.BindTo(this, vm => vm.Fluent);

            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Fluent.IsFocused)
                .Subscribe(_ => Fluent = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Fluent.IsFocused)
                .InvokeCommand(this, vm => vm.Fluent.DeleteFocused);
        }

        /// <inheritdoc />
        public IActionClauseViewModel NewInstance()
        {
            return new FluentSpecificationStatementViewModel();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying action clause model out of the view model.
        /// </summary>
        /// <returns><see cref="FluentSpecificationStatement"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public FluentSpecificationStatement ToModel()
        {
            var fluent = Fluent?.ToModel();

            if (fluent == null)
                throw new MemberNotDefinedException("FluentSpecificationStatementFluentError");

            return new FluentSpecificationStatement(fluent);
        }
    }
}