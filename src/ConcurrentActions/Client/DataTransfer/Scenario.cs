using System.Collections.Generic;
using Model;
using Model.ActionLanguage;
using Model.QueryLanguage;
using Fluent = Model.Fluent;

namespace Client.DataTransfer
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
        /// Manually entered action domain string.
        /// </summary>
        public string ActionDomainInput { get; set; }

        /// <summary>
        /// A set of all query language clauses from the scenario.
        /// </summary>
        public QuerySet QuerySet { get; set; }

        /// <summary>
        /// Manually entered query set string.
        /// </summary>
        public string QuerySetInput { get; set; }

        /// <summary>
        /// Initializes a new <see cref="Scenario"/> instance.
        /// </summary>
        public Scenario()
        {
            Actions = new List<Action>();
            Fluents = new List<Model.Fluent>();
            ActionDomain = new ActionDomain();
            QuerySet = new QuerySet();
        }
    }
}