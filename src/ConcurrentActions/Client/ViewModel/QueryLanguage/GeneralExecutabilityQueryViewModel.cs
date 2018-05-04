using System;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="GeneralExecutabilityQueryView"/> which represents a general exectubaility query in the scenario.
    /// </summary>
    public class GeneralExecutabilityQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralExecutabilityQuery>
    {
        /// <summary>
        /// Keyword describing the query.
        /// </summary>
        public string Label => "executable always";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ExecutabilityQuery";

        /// <summary>
        /// The <see cref="Model.Program"/> instance.
        /// </summary>
        public ProgramViewModel Program { get; set; } = new ProgramViewModel();

        /// <summary>
        /// Command adding a new formula.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, IFormulaViewModel> AddFormula { get; protected set; }

        /// <summary>
        /// Command adding a new program.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddEmptyCompoundAction { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <inheritdoc />
        public ReactiveCommand<ActionViewModel, ActionViewModel> AddAtomicAction { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="GeneralExecutabilityQueryViewModel"/> instance.
        /// </summary>
        public GeneralExecutabilityQueryViewModel()
        {
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => IsFocused || Program.AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddFormulaError"));

            AddEmptyCompoundAction = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.AddEmptyCompoundAction)
                .Where(_ => IsFocused || Program.IsFocused)
                .Subscribe(_ => Program.CompoundActions.Add(new CompoundActionViewModel()));

            AddAtomicAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Program.IsFocused)
                .InvokeCommand(this, vm => vm.Program.DeleteFocused);

            Program.CompoundActions.ItemsAdded.Subscribe(compoundAction =>
            {
                compoundAction.CommandInvocationListeners.Add(
                    this.WhenAnyObservable(vm => vm.AddAtomicAction).InvokeCommand(compoundAction.AddAtomicAction)
                );
                compoundAction.CommandInvocationListeners.Add(
                    this.WhenAnyObservable(vm => vm.DeleteFocused).Where(_ => !IsFocused).InvokeCommand(compoundAction.DeleteFocused)
                );
            });
            Program.CompoundActions.ItemsRemoved.Subscribe(compoundAction => compoundAction.Dispose());
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new GeneralExecutabilityQueryViewModel();
        }

        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="GeneralExecutabilityQuery"/> model represented by given view model.</returns>
        public GeneralExecutabilityQuery ToModel()
        {
            var program = Program?.ToModel();

            if (program == null)
                throw new MemberNotDefinedException("Program in a general executability query is not defined");

            return new GeneralExecutabilityQuery(program);
        }
    }
}