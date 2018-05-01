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
    /// View model for <see cref="ActionView"/> which represents an action from the scenario.
    /// </summary>
    public class ActionViewModel : FodyReactiveObject, IViewModelFor<Action>
    {
        /// <summary>
        /// Underlying <see cref="Model.Action"/> instance.
        /// </summary>
        public Action Action { get; set; }

        /// <inheritdoc />
        public bool IsFocused { get; set; }

        /// <inheritdoc />
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ActionViewModel"/> instance.
        /// </summary>
        public ActionViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ActionViewModel"/> instance
        /// with the supplied <see cref="action"/>.
        /// </summary>
        /// <param name="action">Action instance.</param>
        public ActionViewModel(Action action)
        {
            Action = action;

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
        /// Gets the underlying action model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.Action"/> model represented by given view model.</returns>
        public Action ToModel()
        {
            if (Action == null)
                throw new MemberNotDefinedException("One of the actions is not defined");

            return Action;
        }
    }
}