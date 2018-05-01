using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Client.Abstract;
using Client.Global;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using ReactiveUI;
using Splat;

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

        /// <summary>
        /// Command triggered by delete key used to delete currently selected clause element.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

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
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedFluent)
                .Where(vm => vm != null)
                .Select(fl => new LiteralViewModel(fl.Fluent))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFluent);
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .Where(vm => vm != null)
                .Select(ac => new ActionViewModel(ac.Action))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddAction);
            RibbonViewModel.SelectFormula.InvokeCommand(ActionAreaViewModel, vm => vm.AddFormula);

            RibbonViewModel.Clear.Subscribe(_ =>
            {
                ActionAreaViewModel.ActionDomain.Clear();
                QueryAreaViewModel.QuerySet.Clear();

                var currentScenario = Locator.Current.GetService<ScenarioContainer>();
                currentScenario.ActionViewModels.Clear();
                currentScenario.CompoundActionViewModels.Clear();
                currentScenario.LiteralViewModels.Clear();
                currentScenario.ProgramViewModels.Clear();
            });

            RibbonViewModel.PerformCalculations.Subscribe(_ =>
            {
                // TODO: pass these further to DynamicSystem for evaluation
                var actionDomain = ActionAreaViewModel.GetActionDomainModel();
                var querySet = QueryAreaViewModel.GetQuerySetModel();
            });

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);
            DeleteFocused.Where(_ => Keyboard.IsKeyDown(Key.Delete)).InvokeCommand(ActionAreaViewModel, vm => vm.DeleteFocused);
        }
    }
}