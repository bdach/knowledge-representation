using System.Linq;
using System.Reactive;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.Terminal;
using Model;
using ReactiveUI;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// View model for <see cref="CompoundActionView"/> which represents a compound action from the language signature.
    /// </summary>
    public class CompoundActionViewModel : FodyReactiveObject, IViewModelFor<CompoundAction>
    {
        /// <summary>
        /// List of actions which are part of this compound action.
        /// </summary>
        public ReactiveList<ActionViewModel> Actions { get; set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance.
        /// </summary>
        public CompoundActionViewModel()
        {
            Actions = new ReactiveList<ActionViewModel>();

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {
            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
        }

        /// <summary>
        /// Gets the underlying compound action model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.CompoundAction"/> model represented by given view model.</returns>
        public CompoundAction ToModel()
        {
            var actions = Actions.Select(avm => avm.ToModel()).ToList();
            if (actions.Any(a => a == null))
            {
                throw new MemberNotDefinedException("One of the actions in this compound action has not been defined");
            }
            return new CompoundAction(actions);
        }
    }
}