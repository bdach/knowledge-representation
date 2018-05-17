using System.Collections.Generic;
using Model.QueryLanguage;

namespace DynamicSystem
{
    /// <summary>
    /// Contains the dynamic system's resolution results for the queries supplied.
    /// </summary>
    public class QueryResolution
    {
        /// <summary>
        /// List of results for <see cref="AccessibilityQuery"/> instances.
        /// </summary>
        public List<(AccessibilityQuery, bool)> AccessibilityQueryResults { get; }

        /// <summary>
        /// List of results for <see cref="ExistentialExecutabilityQuery"/> instances.
        /// </summary>
        public List<(ExistentialExecutabilityQuery, bool)> ExistentialExecutabilityQueryResults { get; }

        /// <summary>
        /// List of results for <see cref="GeneralExecutabilityQuery"/> instances.
        /// </summary>
        public List<(GeneralExecutabilityQuery, bool)> GeneralExecutabilityQueryResults { get; }

        /// <summary>
        /// List of results for <see cref="ExistentialValueQuery"/> instances.
        /// </summary>
        public List<(ExistentialValueQuery, bool)> ExistentialValueQueryResults { get; }

        /// <summary>
        /// List of results for <see cref="GeneralValueQuery"/> instances.
        /// </summary>
        public List<(GeneralValueQuery, bool)> GeneralValueQueryResults { get; }

        /// <summary>
        /// Creates an empty instance of the <see cref="QueryResolution"/> class.
        /// </summary>
        public QueryResolution()
        {
            AccessibilityQueryResults = new List<(AccessibilityQuery, bool)>();
            ExistentialExecutabilityQueryResults = new List<(ExistentialExecutabilityQuery, bool)>();
            GeneralExecutabilityQueryResults = new List<(GeneralExecutabilityQuery, bool)>();
            ExistentialValueQueryResults = new List<(ExistentialValueQuery, bool)>();
            GeneralValueQueryResults = new List<(GeneralValueQuery, bool)>();
        }
    }
}