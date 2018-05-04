using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Client.Abstract;
using Client.Global;
using Client.Interface;
using Client.Provider;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using Model.Forms;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <summary>
    /// View model for <see cref="T:Client.View.ShellView" /> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject, IScenarioOwner
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

        public string StatusBarMessage { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ShellViewModel"/> instance.
        /// </summary>
        public ShellViewModel()
        {
            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            RibbonViewModel.PerformCalculations.Subscribe(_ =>
            {
                // TODO: get the list of fluents, clauses, etc. (possibly filter out unused actions and fluents from galleries since deleting is not supported)
            });

            #region Proxying user choices to action area

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
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFormula);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Keyboard.IsKeyDown(Key.Delete))
                .InvokeCommand(ActionAreaViewModel, vm => vm.DeleteFocused);

            #endregion

            #region Proxying user choices to query area

            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedQueryClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => QueryAreaViewModel.QuerySet.Add(t.NewInstance()));
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddFormula);
            this.WhenAnyObservable(vm => vm.RibbonViewModel.AddEmptyCompoundAction)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddEmptyCompoundAction);
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddAtomicAction);
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Keyboard.IsKeyDown(Key.Delete))
                .InvokeCommand(QueryAreaViewModel, vm => vm.DeleteFocused);

            Interactions.StatusBarError.RegisterHandler(interaction =>
            {
                StatusBarMessage = LocalizationProvider.Instance[interaction.Input];
                interaction.SetOutput(Unit.Default);
            });

            #endregion

            #region Backstage support

            RibbonViewModel.Clear.Subscribe(_ =>
            {
                var currentSignature = Locator.Current.GetService<LanguageSignature>();
                currentSignature.Clear();

                ClearActionClauses();
                ClearQueryClauses();
            });

            RibbonViewModel.ImportFromFile.Subscribe(_ =>
            {
                var serializationProvider = new SerializationProvider(this, new ScenarioSerializer());
                serializationProvider.DeserializeScenario();
            });

            RibbonViewModel.ExportToFile.Subscribe(_ =>
            {
                var serializationProvider = new SerializationProvider(this, new ScenarioSerializer());
                serializationProvider.SerializeScenario();
            });

            this.WhenAnyObservable(vm => vm.RibbonViewModel.SetEnglishLocale)
                .Subscribe(msg => StatusBarMessage = "");
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SetPolishLocale)
                .Subscribe(msg => StatusBarMessage = "");

            #endregion
        }

        /// <inheritdoc />
        public void ClearActionClauses()
        {
            ActionAreaViewModel.ActionDomain.Clear();
        }

        /// <inheritdoc />
        public void ClearQueryClauses()
        {
            QueryAreaViewModel.QuerySet.Clear();
        }

        /// <inheritdoc />
        public void ExtendActionClauses(IEnumerable<IActionClauseViewModel> actionClauses)
        {
            ActionAreaViewModel.ActionDomain.AddRange(actionClauses);
        }

        /// <inheritdoc />
        public void ExtendQueryClauses(IEnumerable<IQueryClauseViewModel> queryClauses)
        {
            QueryAreaViewModel.QuerySet.AddRange(queryClauses);
        }

        /// <inheritdoc />
        public Scenario GetCurrentScenario()
        {
            var currentSignature = Locator.Current.GetService<LanguageSignature>();
            return new Scenario
            {
                Actions = currentSignature.ActionViewModels.Select(x => x.ToModel()).ToList(),
                Fluents = currentSignature.LiteralViewModels.Select(x => x.Fluent).ToList(),
                ActionDomain = ActionAreaViewModel.GetActionDomainModel(),
                QuerySet = QueryAreaViewModel.GetQuerySetModel()
            };
        }
    }
}