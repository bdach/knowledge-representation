using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Client.Abstract;
using Client.DataTransfer;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.Provider;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Terminal;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <summary>
    /// View model for <see cref="ShellView" /> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject, IScenarioOwner
    {
        /// <summary>
        /// A global container instance holding the current language signature.
        /// </summary>
        public LanguageSignature LanguageSignature { get; }

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
        /// Message currently displayed in the status bar.
        /// </summary>
        public string StatusBarMessage { get; set; }

        /// <summary>
        /// Initializes a new <see cref="ShellViewModel"/> instance.
        /// </summary>
        /// <param name="languageSignature">Container with the current language signature.</param>
        /// <remarks>
        /// Omitting the <see cref="languageSignature"/> parameter causes the method to fetch the instance registered in the <see cref="Locator"/>.
        /// </remarks>
        public ShellViewModel(LanguageSignature languageSignature = null)
        {
            LanguageSignature = languageSignature ?? Locator.Current.GetService<LanguageSignature>();

            RibbonViewModel = new RibbonViewModel();
            ActionAreaViewModel = new ActionAreaViewModel();
            QueryAreaViewModel = new QueryAreaViewModel();

            DeleteFocused = ReactiveCommand.Create(() => Unit.Default);

            RibbonViewModel.PerformCalculations.Subscribe(_ =>
            {
                // TODO: get the list of fluents, clauses, etc. (possibly filter out unused actions and fluents from galleries since deleting is not supported)
            });

            RibbonViewModel.PerformGrammarCalculations.Subscribe(_ =>
            {
                // TODO: parse input with grammar, show errors, pass to DynamicSystem for evaluation
            });

            Interactions.StatusBarError.RegisterHandler(interaction =>
            {
                StatusBarMessage = LocalizationProvider.Instance[interaction.Input] ?? interaction.Input;
                interaction.SetOutput(Unit.Default);
            });

            RibbonViewModel.EditTabSelected.Subscribe(_ =>
            {
                ActionAreaViewModel.GrammarMode = false;
                QueryAreaViewModel.GrammarMode = false;
            });

            RibbonViewModel.GrammarTabSelected.Subscribe(_ =>
            {
                ActionAreaViewModel.GrammarMode = true;
                QueryAreaViewModel.GrammarMode = true;
            });

            #region Proxying user choices to action area

            // adding new queries
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedActionClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => ActionAreaViewModel.ActionDomain.Add(t.NewInstance()));

            // fluents
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedFluent)
                .Where(vm => vm != null)
                .Select(fl => new LiteralViewModel(fl.Fluent))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFluent);

            // actions
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .Where(vm => vm != null)
                .Select(ac => new ActionViewModel(ac.Action))
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddAction);

            // compound actions
            this.WhenAnyObservable(vm => vm.RibbonViewModel.AddEmptyCompoundAction)
                .Where(_ => ActionAreaViewModel.ActionDomain.Any(acs => acs.AnyChildFocused))
                .Subscribe(_ => Interactions.RaiseStatusBarError("CannotAddCompoundActionError"));

            // formulae
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(ActionAreaViewModel, vm => vm.AddFormula);

            // deletion
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Keyboard.IsKeyDown(Key.Delete))
                .InvokeCommand(ActionAreaViewModel, vm => vm.DeleteFocused);

            #endregion

            #region Proxying user choices to query area

            // adding new queries
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedQueryClauseType)
                .Where(vm => vm != null)
                .Subscribe(t => QueryAreaViewModel.QuerySet.Add(t.NewInstance()));

            // formulae
            this.WhenAnyObservable(vm => vm.RibbonViewModel.SelectFormula)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddFormula);

            // compound actions
            this.WhenAnyObservable(vm => vm.RibbonViewModel.AddEmptyCompoundAction)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddEmptyCompoundAction);

            // actions
            this.WhenAnyValue(vm => vm.RibbonViewModel.SelectedAction)
                .Where(vm => vm != null)
                .InvokeCommand(QueryAreaViewModel, vm => vm.AddAtomicAction);

            // deletion
            this.WhenAnyObservable(vm => vm.DeleteFocused)
                .Where(_ => Keyboard.IsKeyDown(Key.Delete))
                .InvokeCommand(QueryAreaViewModel, vm => vm.DeleteFocused);

            #endregion

            #region Backstage support

            RibbonViewModel.Clear.Subscribe(_ =>
            {
                LanguageSignature.Clear();
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
            ActionAreaViewModel.ActionDomainInput = "";
        }

        /// <inheritdoc />
        public void ClearQueryClauses()
        {
            QueryAreaViewModel.QuerySet.Clear();
            QueryAreaViewModel.QuerySetInput = "";
        }

        /// <inheritdoc />
        public void ExtendActionClauses(string actionDomainInput)
        {
            if (!string.IsNullOrEmpty(actionDomainInput))
            {
                ActionAreaViewModel.ActionDomainInput = !string.IsNullOrEmpty(ActionAreaViewModel.ActionDomainInput)
                    ? $"{ActionAreaViewModel.ActionDomainInput}{Environment.NewLine}{actionDomainInput}"
                    : actionDomainInput;
            }
        }

        /// <inheritdoc />
        public void ExtendActionClauses(IEnumerable<IActionClauseViewModel> actionClauses, string actionDomainInput = null)
        {
            if (actionClauses != null)
            {
                ActionAreaViewModel.ActionDomain.AddRange(actionClauses);
            }

            ExtendActionClauses(actionDomainInput);
        }

        /// <inheritdoc />
        public void ExtendQueryClauses(string querySetInput)
        {
            if (!string.IsNullOrEmpty(querySetInput))
            {
                QueryAreaViewModel.QuerySetInput = !string.IsNullOrEmpty(QueryAreaViewModel.QuerySetInput)
                    ? $"{QueryAreaViewModel.QuerySetInput}{Environment.NewLine}{querySetInput}"
                    : querySetInput;
            }
        }

        /// <inheritdoc />
        public void ExtendQueryClauses(IEnumerable<IQueryClauseViewModel> queryClauses, string querySetInput = null)
        {
            if (queryClauses != null)
            {
                QueryAreaViewModel.QuerySet.AddRange(queryClauses);
            }

            ExtendQueryClauses(querySetInput);
        }

        /// <inheritdoc />
        public Scenario GetCurrentScenario()
        {
            Scenario scenario = null;

            try
            {
                scenario = new Scenario
                {
                    Actions = LanguageSignature.ActionViewModels.Select(x => x.ToModel()).ToList(),
                    Fluents = LanguageSignature.LiteralViewModels.Select(x => ((IViewModelFor<Model.Fluent>)x).ToModel()).ToList(),
                    ActionDomain = ActionAreaViewModel.GetActionDomainModel(),
                    ActionDomainInput = ActionAreaViewModel.ActionDomainInput,
                    QuerySet = QueryAreaViewModel.GetQuerySetModel(),
                    QuerySetInput = QueryAreaViewModel.QuerySetInput
                };
            }
            catch (MemberNotDefinedException ex)
            {
                Interactions.RaiseStatusBarError(ex.Message);
            }

            return scenario;
        }
    }
}