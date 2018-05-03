using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Model;
using ReactiveUI;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// View model for <see cref="ProgramViewModel"/> which represents a program from the language signature.
    /// </summary>
    public class ProgramViewModel : FodyReactiveObject, IViewModelFor<Program>
    {
        /// <summary>
        /// List of compound actions which are part of the program.
        /// </summary>
        public ReactiveList<CompoundActionViewModel> CompoundActions;

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ProgramViewModel"/> instance.
        /// </summary>
        public ProgramViewModel()
        {
            CompoundActions = new ReactiveList<CompoundActionViewModel>();

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {
            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Select(_ => CompoundActions.SingleOrDefault(action => action.IsFocused))
                .Where(action => action != null)
                .Subscribe(action => CompoundActions.Remove(action));
        }

        public Program ToModel()
        {
            var compoundActions = CompoundActions.Select(cavm => cavm.ToModel()).ToList();
            if (compoundActions.Any(ca => ca == null))
            {
                throw new MemberNotDefinedException("One of the compound actions in the program has not been defined");
            }
            return new Program(compoundActions);
        }
    }
}