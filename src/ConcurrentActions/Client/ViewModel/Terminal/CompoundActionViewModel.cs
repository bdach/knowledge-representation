using System.Collections.Generic;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View.Terminal;
using Model;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// View model for <see cref="CompoundActionView"/> which represents a compound action from the scenario.
    /// </summary>
    public class CompoundActionViewModel : FodyReactiveObject, IViewModelFor<CompoundAction>
    {
        /// <summary>
        /// Underlying <see cref="Model.CompoundAction"/> instance.
        /// </summary>
        public CompoundAction CompoundAction { get; set; }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance.
        /// </summary>
        public CompoundActionViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance
        /// with the supplied <see cref="action"/>.
        /// </summary>
        /// <param name="action">Action instance.</param>
        public CompoundActionViewModel(Action action)
        {
            CompoundAction = new CompoundAction(action);

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance
        /// with the supplied <see cref="compoundAction"/>.
        /// </summary>
        /// <param name="compoundAction">Compound action instance.</param>
        public CompoundActionViewModel(CompoundAction compoundAction)
        {
            CompoundAction = compoundAction;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance
        /// with the supplied list of <see cref="actions"/>.
        /// </summary>
        /// <param name="actions">Collection of actions.</param>
        public CompoundActionViewModel(IEnumerable<Action> actions)
        {
            CompoundAction = new CompoundAction(actions);

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="CompoundActionViewModel"/> instance
        /// with the supplied list of <see cref="actions"/> filtered by <see cref="selection"/>.
        /// </summary>
        /// <param name="actions">Collection of actions.</param>
        /// <param name="selection">List of booleans to select actions.</param>
        public CompoundActionViewModel(ICollection<Action> actions, ICollection<bool> selection)
        {
            CompoundAction = new CompoundAction(actions, selection);

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {

        }

        /// <summary>
        /// Gets the underlying compound action model out of the view model.
        /// </summary>
        /// <returns><see cref="Model.CompoundAction"/> model represented by given view model.</returns>
        public CompoundAction ToModel()
        {
            if (CompoundAction == null)
                throw new MemberNotDefinedException("One of the compound actions is not defined");
            // TODO: check if exists but is empty? or make sure that empty cannot be added in the action creation modal

            return CompoundAction;
        }
    }
}