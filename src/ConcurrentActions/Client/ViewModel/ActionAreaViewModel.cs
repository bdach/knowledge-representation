using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Interface;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using ReactiveUI;

namespace Client.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// View model for <see cref="ActionAreaView"/> which manages the list of actions.
    /// </summary>
    public class ActionAreaViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Collection containing all Action Language clauses.
        /// </summary>
        public ObservableCollection<IActionClauseViewModel> ActionDomain { get; protected set; }

        /// <summary>
        /// Command used for adding an action to an item of this collection.
        /// </summary>
        public ReactiveCommand<ActionViewModel, Unit> AddAction;

        /// <summary>
        /// Command used for adding a fluent to an item of this collection.
        /// </summary>
        public ReactiveCommand<LiteralViewModel, Unit> AddFluent;

        /// <summary>
        /// Command used for adding a formula to an item of this collection.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula;

        /// <summary>
        /// Initializes a new <see cref="ActionAreaViewModel"/> instance.
        /// </summary>
        public ActionAreaViewModel()
        {
            ActionDomain = new ObservableCollection<IActionClauseViewModel>();

            AddAction = ReactiveCommand.Create((ActionViewModel ac) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => ac).InvokeCommand(viewModel.AddAction);
                }
            }, null, RxApp.MainThreadScheduler);  // WARNING: Do not remove this, lest you get threading errors.

            AddFluent = ReactiveCommand.Create((LiteralViewModel fl) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => fl).InvokeCommand(viewModel.AddFluent);
                }
            }, null, RxApp.MainThreadScheduler);

            AddFormula = ReactiveCommand.Create((IFormulaViewModel form) =>
            {
                foreach (var viewModel in ActionDomain)
                {
                    Observable.Start(() => form).InvokeCommand(viewModel.AddFormula);
                }
            }, null, RxApp.MainThreadScheduler);
        }
    }
}