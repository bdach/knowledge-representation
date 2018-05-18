using System.Collections.Generic;

namespace Model.QueryLanguage
{
    /// <summary>
    /// Represents a set of queries to be resolved by the dynamic system.
    /// </summary>
    public class QuerySet
    {
        /// <summary>
        /// List of all <see cref="AccessibilityQuery"/> objects in the query set.
        /// </summary>
        public List<AccessibilityQuery> AccessibilityQueries { get; }

        /// <summary>
        /// List of all <see cref="ExistentialExecutabilityQuery"/> objects in the query set.
        /// </summary>
        public List<ExistentialExecutabilityQuery> ExistentialExecutabilityQueries { get; }

        /// <summary>
        /// List of all <see cref="GeneralExecutabilityQuery"/> objects in the query set.
        /// </summary>
        public List<GeneralExecutabilityQuery> GeneralExecutabilityQueries { get; }

        /// <summary>
        /// List of all <see cref="ExistentialValueQuery"/> objects in the query set.
        /// </summary>
        public List<ExistentialValueQuery> ExistentialValueQueries { get; }

        /// <summary>
        /// List of all <see cref="GeneralValueQuery"/> objects in the query set.
        /// </summary>
        public List<GeneralValueQuery> GeneralValueQueries { get; }

        /// <summary>
        /// Initializes a new, empty instance of the <see cref="QuerySet"/> class.
        /// </summary>
        public QuerySet()
        {
            AccessibilityQueries = new List<AccessibilityQuery>();
            ExistentialExecutabilityQueries = new List<ExistentialExecutabilityQuery>();
            GeneralExecutabilityQueries = new List<GeneralExecutabilityQuery>();
            ExistentialValueQueries = new List<ExistentialValueQuery>();
            GeneralValueQueries = new List<GeneralValueQuery>();
        }

        public IEnumerable<Action> Actions()
        {
            var actions = new HashSet<Action>();
            ExistentialExecutabilityQueries.ForEach(q =>
                q.Program.Actions.ForEach(ca => actions.UnionWith(ca.Actions)));
            GeneralExecutabilityQueries.ForEach(q => q.Program.Actions.ForEach(ca => actions.UnionWith(ca.Actions)));
            ExistentialValueQueries.ForEach(q => q.Program.Actions.ForEach(ca => actions.UnionWith(ca.Actions)));
            GeneralValueQueries.ForEach(q => q.Program.Actions.ForEach(ca => actions.UnionWith(ca.Actions)));
            return actions;
        }

        public IEnumerable<Fluent> Fluents()
        {
            var fluents = new HashSet<Fluent>();
            AccessibilityQueries.ForEach(q => fluents.UnionWith(q.Target.Fluents));
            ExistentialValueQueries.ForEach(q => fluents.UnionWith(q.Target.Fluents));
            GeneralValueQueries.ForEach(q => fluents.UnionWith(q.Target.Fluents));
            return fluents;
        }
    }
}