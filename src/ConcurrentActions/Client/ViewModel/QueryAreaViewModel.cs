using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Client.Abstract;
using Client.Exception;
using Client.Interface;
using Client.View;
using Client.ViewModel.Grammar;
using Client.ViewModel.Terminal;
using DynamicSystem;
using Model.QueryLanguage;
using ReactiveUI;

namespace Client.ViewModel
{
    /// <inheritdoc />
    /// <summary>
    /// View model for <see cref="QueryAreaView"/> which manages the list of queries.
    /// </summary>
    public class QueryAreaViewModel : FodyReactiveObject
    {
        /// <summary>
        /// Collection containing all Query Language clauses.
        /// </summary>
        /// <remarks>
        /// This is not a *SET*. You have been bamboozled.
        /// </remarks>
        public ReactiveList<IQueryClauseViewModel> QuerySet { get; protected set; }

        /// <summary>
        /// String bound to user input from the QueryAreaBox.
        /// </summary>
        public string QuerySetInput { get; set; } = "";

        /// <summary>
        /// Command used to add formulae to the query clauses.
        /// </summary>
        public ReactiveCommand<IFormulaViewModel, Unit> AddFormula { get; protected set; }

        /// <summary>
        /// Command used to add empty compound actions to the query clauses.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddEmptyCompoundAction { get; protected set; }

        /// <summary>
        /// Command used to close query result view.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseQueryResults { get; protected set; }

        /// <summary>
        /// Command used to delete the currently focused item.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteFocused { get; protected set; }

        /// <summary>
        /// Command used to forward atomic actions that are to be added to compound actions.
        /// </summary>
        public ReactiveCommandBase<ActionViewModel, Unit> AddAtomicAction { get; protected set; }

        /// <summary>
        /// Indicates whether the grammar tab is selected.
        /// </summary>
        public bool GrammarMode { get; set; }

        /// <summary>
        /// Indicates whether the grammar query results are shown
        /// </summary>
        public bool GrammarViewResults { get; set; }

        /// <summary>
        /// Collection containing all grammar query results.
        /// </summary>
        public ReactiveList<QueryResultViewModel> EvaluationResults { get; set; }

        /// <summary>
        /// Initializes a new <see cref="QueryAreaViewModel"/> instance.
        /// </summary>
        public QueryAreaViewModel()
        {
            QuerySet = new ReactiveList<IQueryClauseViewModel>();

            AddFormula = ReactiveCommand.Create<IFormulaViewModel>(formula =>
            {
                foreach (var viewModel in QuerySet)
                {
                    Observable.Start(() => formula).InvokeCommand(viewModel, vm => vm.AddFormula);
                }
            }, null, RxApp.MainThreadScheduler);

            AddEmptyCompoundAction = ReactiveCommand.Create<Unit>(_ =>
            {
                foreach (var viewModel in QuerySet)
                {
                    Observable.Start(() => Unit.Default).InvokeCommand(viewModel, vm => vm.AddEmptyCompoundAction);
                }
            }, null, RxApp.MainThreadScheduler);

            AddAtomicAction = ReactiveCommand.Create<ActionViewModel>(action =>
            {
                foreach (var viewModel in QuerySet)
                {
                    Observable.Start(() => action).InvokeCommand(viewModel, vm => vm.AddAtomicAction);
                }
            });

            CloseQueryResults = ReactiveCommand.Create(() =>
            {
                this.GrammarViewResults = false;
                this.EvaluationResults = null;
            });

            DeleteFocused = ReactiveCommand.Create(() =>
            {
                foreach (var viewModel in QuerySet)
                {
                    if (viewModel.IsFocused)
                    {
                        QuerySet.Remove(viewModel);
                        return;
                    }
                }

                // these foreach loops are separated to make sure that we propagate deletion only if no clauses are selected
                foreach (var viewModel in QuerySet)
                {
                    Observable.Start(() => Unit.Default).InvokeCommand(viewModel.DeleteFocused);
                }
            }, null, RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// Converts all query clauses to corresponding models and combines
        /// them into <see cref="QuerySet"/> instance.
        /// </summary>
        /// <returns><see cref="QuerySet"/> instance with all query clauses from the current scenario.</returns>
        /// <exception cref="MemberNotDefinedException">Thrown when one of the view model members is null or a placeholder.</exception>
        public QuerySet GetQuerySetModel()
        {
            var querySet = new QuerySet();

            foreach (var actionClause in QuerySet)
            {
                switch (actionClause)
                {
                    case IViewModelFor<AccessibilityQuery> accesibilityQuery:
                        querySet.AccessibilityQueries.Add(accesibilityQuery.ToModel());
                        break;
                    case IViewModelFor<ExistentialExecutabilityQuery> existentialExecutabilityQuery:
                        querySet.ExistentialExecutabilityQueries.Add(existentialExecutabilityQuery.ToModel());
                        break;
                    case IViewModelFor<GeneralExecutabilityQuery> generalExecutabilityQuery:
                        querySet.GeneralExecutabilityQueries.Add(generalExecutabilityQuery.ToModel());
                        break;
                    case IViewModelFor<ExistentialValueQuery> existentialValueQuery:
                        querySet.ExistentialValueQueries.Add(existentialValueQuery.ToModel());
                        break;
                    case IViewModelFor<GeneralValueQuery> generalValueQuery:
                        querySet.GeneralValueQueries.Add(generalValueQuery.ToModel());
                        break;
                }
            }

            return querySet;
        }

        /// <summary>
        /// Forwards the query results to the viewmodels, which store them in the <see cref="IQueryClauseViewModel.Result"/> property.
        /// </summary>
        /// <param name="results"></param>
        public void AcceptResults(QueryResolution results)
        {
            foreach (var queryClauseViewModel in QuerySet)
            {
                queryClauseViewModel.AcceptResult(results);
            }
        }
    }
}