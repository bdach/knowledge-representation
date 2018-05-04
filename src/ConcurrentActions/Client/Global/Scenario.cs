using System.Collections.Generic;
using Model;
using Model.ActionLanguage;
using Model.QueryLanguage;

namespace Client.Global
{
    /// <summary>
    /// Container holding models of items from language signature and clauses.
    /// </summary>
    public class Scenario
    {
        /// <summary>
        /// List of <see cref="Action"/>s that are in the language signature.
        /// </summary>
        public List<Action> Actions { get; set; }

        /// <summary>
        /// List of <see cref="Model.Fluent"/>s that are in the language signature.
        /// </summary>
        public List<Model.Fluent> Fluents { get; set; }

        /// <summary>
        /// A set of all action language clauses from the scenario.
        /// </summary>
        public ActionDomain ActionDomain { get; set; }

        /// <summary>
        /// A set of all query language clauses from the scenario.
        /// </summary>
        public QuerySet QuerySet { get; set; }
    }
}