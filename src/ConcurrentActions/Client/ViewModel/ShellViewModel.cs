using System;
using System.Reactive.Linq;
using Client.Abstract;
using Client.View;
using Client.ViewModel.Terminal;
using ReactiveUI;

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

        #endregion

        /// <summary>
        /// Initializes a new <see cref="ShellViewModel"/> instance.
        /// </summary>
        public ShellViewModel()
        {
            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();

            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedQueryClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => QueryAreaViewModel.QuerySet.Add(t.NewInstance()));
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedActionClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => ActionAreaViewModel.ActionDomain.Add(t.NewInstance()));
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .Where(vm => vm != null)
                .Select(ac => new ActionViewModel(ac.Action))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddAction);
        }
    }
}