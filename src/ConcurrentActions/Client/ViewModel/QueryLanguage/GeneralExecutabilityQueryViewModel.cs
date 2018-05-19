using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.View.QueryLanguage;
using Client.ViewModel.Terminal;
using DynamicSystem;
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
        public bool AnyChildFocused => IsFocused || Program.AnyChildFocused;

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
                .Where(_ => Program.CompoundActions.Any(compundAction => compundAction.AnyChildFocused))
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddCompoundActionError"));
            this.WhenAnyObservable(vm => vm.AddEmptyCompoundAction)
                .Where(_ => IsFocused || Program.IsFocused)
                .Subscribe(_ => Program.CompoundActions.Add(new CompoundActionViewModel()));

            AddAtomicAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            this.WhenAnyObservable(vm => vm.AddAtomicAction)
                .Where(_ => Program.IsFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !Program.IsFocused)
                .InvokeCommand(this, vm => vm.Program.DeleteFocused);

            this.WhenAnyValue(vm => vm.Program)
                .Select(program => program.CompoundActions)
                .Subscribe(compoundActions =>
                {
                    foreach (var compoundActionViewModel in compoundActions)
                    {
                        RegisterListeners(compoundActionViewModel);
                    }
                });
            this.WhenAnyObservable(vm => vm.Program.CompoundActions.ItemsAdded)
                .Subscribe(RegisterListeners);
            this.WhenAnyObservable(vm => vm.Program.CompoundActions.ItemsRemoved)
                .Subscribe(compoundAction => compoundAction.Dispose());
        }

        /// <summary>
        /// Registers the command invocation listener for a new compound action.
        /// </summary>
        /// <param name="compoundAction">The newly added <see cref="CompoundActionViewModel"/> instance.</param>
        private void RegisterListeners(CompoundActionViewModel compoundAction)
        {
            compoundAction.CommandInvocationListeners.Add(this.WhenAnyObservable(vm => vm.AddAtomicAction).InvokeCommand(compoundAction.AddAtomicAction));
            compoundAction.CommandInvocationListeners.Add(this.WhenAnyObservable(vm => vm.DeleteFocused).Where(_ => !IsFocused).InvokeCommand(compoundAction.DeleteFocused));
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new GeneralExecutabilityQueryViewModel();
        }

        // TODO: this should be cleared upon any changes
        /// <inheritdoc />
        public bool? Result { get; set; }

        /// <inheritdoc />
        public void AcceptResult(QueryResolution results)
        {
            // TODO: SUPER dirty
            Result = results.GeneralExecutabilityQueryResults.First().Item2;
            results.GeneralExecutabilityQueryResults.RemoveAt(0);
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="GeneralExecutabilityQuery"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public GeneralExecutabilityQuery ToModel()
        {
            var program = Program?.ToModel();

            if (program == null)
                throw new MemberNotDefinedException("GeneralExecutabilityQueryProgramError");

            return new GeneralExecutabilityQuery(program);
        }
    }
}