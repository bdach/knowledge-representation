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
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel.QueryLanguage
{
    /// <summary>
    /// View model for <see cref="GeneralValueQueryView"/> which represents a general value query in the scenario.
    /// </summary>
    public class GeneralValueQueryViewModel : FodyReactiveObject, IQueryClauseViewModel, IViewModelFor<GeneralValueQuery>
    {
        /// <summary>
        /// First keyword describing the query.
        /// </summary>
        public string LabelLeft => "necessary";

        /// <inheritdoc />
        public string ClauseTypeNameKey => "ValueQuery";

        /// <summary>
        /// Second keyword describing the query.
        /// </summary>
        public string LabelRight => "after";

        /// <summary>
        /// The <see cref="IViewModelFor{T}"/> instance returning a target formula.
        /// </summary>
        public IFormulaViewModel Target { get; set; } = new PlaceholderViewModel();

        /// <summary>
        /// The <see cref="ProgramViewModel"/> instance.
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
        public ReactiveCommand<ActionViewModel, ActionViewModel> AddAtomicAction { get; protected set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public bool AnyChildFocused => IsFocused || Target.AnyChildFocused || Program.AnyChildFocused;

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="GeneralValueQueryViewModel"/> instance.
        /// </summary>
        public GeneralValueQueryViewModel()
        {
            AddFormula = ReactiveCommand.Create<IFormulaViewModel, IFormulaViewModel>(formula => formula);
            AddFormula.Subscribe(InsertFormula);
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => Program.AnyChildFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddFormulaError"));
            this.WhenAnyObservable(vm => vm.AddFormula)
                .Where(_ => !IsFocused)
                .InvokeCommand(this, vm => vm.Target.AddFormula);

            AddEmptyCompoundAction = ReactiveCommand.Create(() => Unit.Default);
            this.WhenAnyObservable(vm => vm.AddEmptyCompoundAction)
                .Where(_ => Target.AnyChildFocused || Program.CompoundActions.Any(compundAction => compundAction.AnyChildFocused))
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddCompoundActionError"));
            this.WhenAnyObservable(vm => vm.AddEmptyCompoundAction)
                .Where(_ => IsFocused || Program.IsFocused)
                .Subscribe(_ => Program.CompoundActions.Add(new CompoundActionViewModel()));

            AddAtomicAction = ReactiveCommand.Create<ActionViewModel, ActionViewModel>(action => action);
            this.WhenAnyObservable(vm => vm.AddAtomicAction)
                .Where(_ => Target.AnyChildFocused || Program.IsFocused)
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddActionError"));

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Target.IsFocused)
                .Subscribe(_ => Target = new PlaceholderViewModel());

            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Target.IsFocused || Program.IsFocused))
                .InvokeCommand(this, vm => vm.Program.DeleteFocused);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => !(Target.IsFocused || Program.IsFocused))
                .InvokeCommand(this, vm => vm.Target.DeleteFocused);

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

        private void RegisterListeners(CompoundActionViewModel compoundAction)
        {
            compoundAction.CommandInvocationListeners.Add(this.WhenAnyObservable(vm => vm.AddAtomicAction).InvokeCommand(compoundAction.AddAtomicAction));
            compoundAction.CommandInvocationListeners.Add(this.WhenAnyObservable(vm => vm.DeleteFocused).Where(_ => !IsFocused).InvokeCommand(compoundAction.DeleteFocused));
        }

        public void InsertFormula(IFormulaViewModel formula)
        {
            if (Target.IsFocused)
            {
                Target = formula.Accept(Target);
            }
        }

        /// <inheritdoc />
        public IQueryClauseViewModel NewInstance()
        {
            return new GeneralValueQueryViewModel();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the underlying query model out of the view model.
        /// </summary>
        /// <returns><see cref="GeneralValueQuery"/> model represented by given view model.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown if one of the view model members is null or a placeholder.</exception>
        public GeneralValueQuery ToModel()
        {
            var target = Target?.ToModel();
            var program = Program?.ToModel();

            if (target == null)
                throw new MemberNotDefinedException("Target in a general value query is not defined");
            if (program == null)
                throw new MemberNotDefinedException("Program in a general value query is not defined");

            return new GeneralValueQuery(target, program);
        }
    }
}