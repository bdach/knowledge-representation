﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Client.Abstract;
using Client.DataTransfer;
using Client.Exception;
using Client.Global;
using Client.Interface;
using Client.Provider;
using Client.View;
using Client.ViewModel.Formula;
using Client.ViewModel.Grammar;
using Client.ViewModel.Terminal;
using DynamicSystem;
using DynamicSystem.Grammar;
using Model;
using Model.ActionLanguage;
using Model.QueryLanguage;
using ReactiveUI;
using Splat;

namespace Client.ViewModel
{
    /// <summary>
    /// View model for <see cref="ShellView" /> which is the root view of the application.
    /// </summary>
    public class ShellViewModel : FodyReactiveObject, IInputOwner
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
            
            RibbonViewModel.PerformCalculations = ReactiveCommand.CreateFromObservable(() => Observable.StartAsync(token => Task.Run(() =>
            {
                var scenario = GetCurrentScenario();
                if (scenario != null)
                {
                    var signature = new Signature(scenario.Fluents, scenario.Actions);
                    return QueryResolver.ResolveQueries(signature, scenario.ActionDomain, scenario.QuerySet);
                }
                return null;
            }, token)).TakeUntil(RibbonViewModel.CancelCalculations));
            RibbonViewModel.PerformCalculations
                .Where(results => results != null)
                .Subscribe(results => QueryAreaViewModel.AcceptResults(results));
            RibbonViewModel.PerformCalculations.ThrownExceptions.Subscribe(ex =>
                Interactions.RaiseStatusBarError(ex.Message));

            RibbonViewModel.PerformGrammarCalculations = ReactiveCommand.CreateFromObservable(() => Observable.StartAsync(token => Task.Run(() =>
            {
                Action<string> raiseError = err => Application.Current.Dispatcher.Invoke(() => Interactions.RaiseStatusBarError(err));

                ActionDomain actionDomain;
                QuerySet querySet;
                Dictionary<object, int> queryOrder;

                try
                {
                    actionDomain = DynamicSystemParserUtils.ParseActionDomain(ActionAreaViewModel.ActionDomainInput);
                }
                catch (System.Exception e)
                {
                    raiseError($"{LocalizationProvider.Instance["ActionDomainParseErrorPrefix"]}: {e.Message}");
                    return null;
                }

                try
                {
                    (querySet, queryOrder) = DynamicSystemParserUtils.ParseQuerySetWithOrder(QueryAreaViewModel.QuerySetInput);
                }
                catch (System.Exception e)
                {
                    raiseError($"{LocalizationProvider.Instance["QuerySetParseErrorPrefix"]}: {e.Message}");
                    return null;
                }

                var actionDomainFluents = actionDomain.Fluents();
                var actionDomainActions = actionDomain.Actions();

                var querySetActions = querySet.Actions();
                var querySetFluents = querySet.Fluents();

                var notFoundFluent = querySetFluents.FirstOrDefault(f => !actionDomainFluents.Contains(f));
                if (notFoundFluent != null)
                {
                    raiseError(
                        $"{LocalizationProvider.Instance["FluentNotDefinedInActionDomainPrefix"]}" +
                        $" \"{notFoundFluent.Name}\" " +
                        $"{LocalizationProvider.Instance["FluentNotDefinedInActionDomainSuffix"]}"
                    );
                    return null;
                }

                var notFoundAction = querySetActions.FirstOrDefault(a => !actionDomainActions.Contains(a));
                if (notFoundAction != null)
                {
                    raiseError(
                        $"{LocalizationProvider.Instance["ActionNotDefinedInActionDomainPrefix"]}" +
                        $" \"{notFoundAction.Name}\" " +
                        $"{LocalizationProvider.Instance["ActionNotDefinedInActionDomainSuffix"]}"
                    );
                    return null;
                }

                var signature = new Signature(actionDomainFluents, actionDomainActions);
                return new Tuple<QueryResolution, Dictionary<object ,int>>(QueryResolver.ResolveQueries(signature, actionDomain, querySet), queryOrder);
            }, token)).TakeUntil(RibbonViewModel.CancelCalculations));
            RibbonViewModel.PerformGrammarCalculations.ThrownExceptions.Subscribe(ex =>
                Interactions.RaiseStatusBarError(ex.Message));

            RibbonViewModel.PerformGrammarCalculations
                .Where(results => results != null)
                .Subscribe(data =>
                {
                    var results = data.Item1;
                    var queryOrder = data.Item2.Select(v => (v.Key.ToString(), v.Value))
                        .ToDictionary(v => v.Item1, v => v.Item2);

                    var queryResult = results.AccessibilityQueryResults.Select(r => (r.Item1.ToString(), r.Item2))
                        .Concat(results.ExistentialExecutabilityQueryResults.Select(r => (r.Item1.ToString(), r.Item2)))
                        .Concat(results.ExistentialValueQueryResults.Select(r => (r.Item1.ToString(), r.Item2)))
                        .Concat(results.GeneralExecutabilityQueryResults.Select(r => (r.Item1.ToString(), r.Item2)))
                        .Concat(results.GeneralValueQueryResults.Select(r => (r.Item1.ToString(), r.Item2)))
                        .OrderBy(q => queryOrder[q.Item1])
                        .Select(qr => new QueryResultViewModel(qr.Item1, qr.Item2));

                    QueryAreaViewModel.GrammarViewResults = true;
                    QueryAreaViewModel.EvaluationResults = new ReactiveList<QueryResultViewModel>(queryResult.ToList());
                }, e => MessageBox.Show(e.Message));

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
                ClearActionInput();
                ClearQueryInput();
            });

            RibbonViewModel.ImportFromFile.Subscribe(_ =>
            {
                var serializationProvider = new SerializationProvider(this, new ScenarioSerializer(), new GrammarSerializer());
                serializationProvider.DeserializeInput();
            });

            RibbonViewModel.ExportToFile.Subscribe(_ =>
            { 
                var serializationProvider = new SerializationProvider(this, new ScenarioSerializer(), new GrammarSerializer());
                serializationProvider.SerializeInput();
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
            if (actionClauses != null)
            {
                ActionAreaViewModel.ActionDomain.AddRange(actionClauses);
            }
        }


        /// <inheritdoc />
        public void ExtendQueryClauses(IEnumerable<IQueryClauseViewModel> queryClauses)
        {
            if (queryClauses != null)
            {
                QueryAreaViewModel.QuerySet.AddRange(queryClauses);
            }
        }

        /// <inheritdoc />
        public Scenario GetCurrentScenario()
        {
            return new Scenario
            {
                Actions = LanguageSignature.ActionViewModels.Select(x => x.ToModel()).ToList(),
                Fluents = LanguageSignature.LiteralViewModels
                    .Select(x => ((IViewModelFor<Model.Fluent>) x).ToModel()).ToList(),
                ActionDomain = ActionAreaViewModel.GetActionDomainModel(),
                QuerySet = QueryAreaViewModel.GetQuerySetModel(),
            };
        }

        /// <inheritdoc />
        public void ClearActionInput()
        {
            ActionAreaViewModel.ActionDomainInput = "";
        }

        /// <inheritdoc />
        public void ClearQueryInput()
        {
            QueryAreaViewModel.QuerySetInput = "";
        }

        /// <inheritdoc />
        public void ExtendActionInput(string actionDomainInput)
        {
            if (!string.IsNullOrEmpty(actionDomainInput))
            {
                ActionAreaViewModel.ActionDomainInput = !string.IsNullOrEmpty(ActionAreaViewModel.ActionDomainInput)
                    ? $"{ActionAreaViewModel.ActionDomainInput}{Environment.NewLine}{actionDomainInput}"
                    : actionDomainInput;
            }
        }

        /// <inheritdoc />
        public void ExtendQueryInput(string querySetInput)
        {
            if (!string.IsNullOrEmpty(querySetInput))
            {
                QueryAreaViewModel.QuerySetInput = !string.IsNullOrEmpty(QueryAreaViewModel.QuerySetInput)
                    ? $"{QueryAreaViewModel.QuerySetInput}{Environment.NewLine}{querySetInput}"
                    : querySetInput;
            }
        }

        /// <inheritdoc />
        public GrammarInput GetCurrentGrammar()
        {
            GrammarInput grammarInput = null;

            try
            {
                grammarInput = new GrammarInput()
                {
                    QuerySetInput = QueryAreaViewModel.QuerySetInput,
                    ActionDomainInput = ActionAreaViewModel.ActionDomainInput
                };
            }
            catch (MemberNotDefinedException ex)
            {
                Interactions.RaiseStatusBarError(ex.Message);
            }

            return grammarInput;
        }

        /// <inheritdoc />
        public bool GrammarMode
        {
            get => ActionAreaViewModel.GrammarMode && QueryAreaViewModel.GrammarMode;
            set
            {
                ActionAreaViewModel.GrammarMode = value;
                QueryAreaViewModel.GrammarMode = value;

                RibbonViewModel.IsGrammarTabSelected = value;
                RibbonViewModel.IsEditTabSelected = !value;
            }
        }

    }
}