using System.Collections.ObjectModel;
using Client.Abstract;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model;
using Model.Forms;

namespace Client.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// View model for <see cref="T:Client.View.ShellView" /> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject
    {
        /// <summary>
        /// View model of nested <see cref="RibbonView"/>.
        /// </summary>
        public RibbonViewModel RibbonViewModel { get; set; }

        /// <summary>
        /// View model of nested <see cref="ActionAreaView"/>.
        /// </summary>
        public ActionAreaViewModel ActionAreaViewModel { get; set; }

        /// <summary>
        /// View model of nested <see cref="QueryAreaView"/>.
        /// </summary>
        public QueryAreaViewModel QueryAreaViewModel { get; set; }

        // TODO: move the following four observables into a VM wrapping *AreaViewModels
        #region TODO: Scheduled for eviction

        /// <summary>
        /// Collection of all <see cref="ActionViewModel"/>s for <see cref="Action"/>s defined in the scenario.
        /// </summary>
        public ObservableCollection<ActionViewModel> ActionViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="CompoundActionViewModel"/>s for <see cref="CompoundAction"/>s defined in the scenario.
        /// </summary>
        /// <remarks>
        /// In fact, the <see cref="CompoundActionViewModel"/> contains a collection of <see cref="ActionViewModel"/>,
        /// but I wanted the toolip to look cool.
        /// </remarks>
        public ObservableCollection<CompoundActionViewModel> CompoundActionViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="LiteralViewModel"/>s for <see cref="Literal"/>s defined in the scenario.
        /// </summary>
        public ObservableCollection<LiteralViewModel> LiteralViewModels { get; protected set; }

        /// <summary>
        /// Collection of all <see cref="ProgramViewModel"/>s for <see cref="Program"/>s defined in the scenario.
        /// </summary>
        public ObservableCollection<ProgramViewModel> ProgramViewModels { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="ShellViewModel"/> instance.
        /// </summary>
        public ShellViewModel()
        {
            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();

            ActionViewModels = new ObservableCollection<ActionViewModel>();
            CompoundActionViewModels = new ObservableCollection<CompoundActionViewModel>();
            LiteralViewModels = new ObservableCollection<LiteralViewModel>();
            ProgramViewModels = new ObservableCollection<ProgramViewModel>();
        }
    }
}