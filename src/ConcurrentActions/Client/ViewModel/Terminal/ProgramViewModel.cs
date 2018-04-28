using System.Collections.Generic;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Model;

namespace Client.ViewModel.Terminal
{
    /// <summary>
    /// View model for <see cref="ProgramViewModel"/> which represents a program from the scenario.
    /// </summary>
    public class ProgramViewModel : FodyReactiveObject, IViewModelFor<Program>
    {
        /// <summary>
        /// Underlying <see cref="Model.Program"/> instance.
        /// </summary>
        public Program Program { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ProgramViewModel"/> instance.
        /// </summary>
        public ProgramViewModel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ProgramViewModel"/> instance
        /// with the supplied <see cref="program"/>.
        /// </summary>
        /// <param name="program">Program instance.</param>
        public ProgramViewModel(Program program)
        {
            Program = program;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new <see cref="ProgramViewModel"/> instance
        /// with the supplied list of <see cref="actions"/>.
        /// </summary>
        /// <param name="actions">List of actions.</param>
        public ProgramViewModel(IEnumerable<CompoundAction> actions)
        {
            Program = new Program(actions);

            InitializeComponent();
        }

        /// <summary>
        /// Private method which contains instructions common to all constructors.
        /// </summary>
        private void InitializeComponent()
        {

        }

        public Program ToModel()
        {
            if (Program == null)
                throw new MemberNotDefinedException("One of the programs is not defined");
            // TODO: check if exists but is empty? or make sure that empty cannot be added in the program creation modal

            return Program;
        }
    }
}