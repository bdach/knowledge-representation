﻿using System.Collections.ObjectModel;
using Client.Abstract;
using Client.Interface;
using Client.View;
using Model.QueryLanguage;

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
        public ObservableCollection<IQueryClauseViewModel> QuerySet { get; protected set; }

        /// <summary>
        /// Initializes a new <see cref="QueryAreaViewModel"/> instance.
        /// </summary>
        public QueryAreaViewModel()
        {
            QuerySet = new ObservableCollection<IQueryClauseViewModel>();
        }

        /// <summary>
        /// Converts all query clauses to corresponding models and combines
        /// them into <see cref="QuerySet"/> instance.
        /// </summary>
        /// <returns><see cref="QuerySet"/> instance with all query clauses from the current scenario.</returns>
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
    }
}