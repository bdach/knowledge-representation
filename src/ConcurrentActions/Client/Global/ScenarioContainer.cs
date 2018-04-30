using System.Collections.ObjectModel;
using Client.Abstract;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model;
using Model.Forms;
using ReactiveUI;

namespace Client.Global
{
    /// <summary>
    /// Singleton class used to store all the information about current scenario.
    /// </summary>
    public class ScenarioContainer : FodyReactiveObject
    {
        /// <summary>
        /// Collection of all <see cref="ActionViewModel"/>s for <see cref="Action"/>s defined in the scenario.
        /// </summary>
        public ReactiveList<ActionViewModel> ActionViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="CompoundActionViewModel"/>s for <see cref="CompoundAction"/>s defined in the scenario.
        /// </summary>
        /// <remarks>
        /// In fact, the <see cref="CompoundActionViewModel"/> contains a collection of <see cref="ActionViewModel"/>,
        /// but I wanted the toolip to look cool.
        /// </remarks>
        public ReactiveList<CompoundActionViewModel> CompoundActionViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="LiteralViewModel"/>s for <see cref="Literal"/>s defined in the scenario.
        /// </summary>
        public ReactiveList<LiteralViewModel> LiteralViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="ProgramViewModel"/>s for <see cref="Program"/>s defined in the scenario.
        /// </summary>
        public ReactiveList<ProgramViewModel> ProgramViewModels { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="ScenarioContainer"/> instance.
        /// </summary>
        public ScenarioContainer()
        {
            ActionViewModels = new ReactiveList<ActionViewModel>();
            CompoundActionViewModels = new ReactiveList<CompoundActionViewModel>();
            LiteralViewModels = new ReactiveList<LiteralViewModel>();
            ProgramViewModels = new ReactiveList<ProgramViewModel>();
        }
    }
}