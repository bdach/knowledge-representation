using System.Collections.Generic;
using Client.DataTransfer;
using Client.ViewModel;

namespace Client.Interface
{
    /// <summary>
    /// Interface implemented by the root <see cref="ShellViewModel"/>
    /// which allows for scenario management
    /// </summary>
    public interface IScenarioOwner
    {
        /// <summary>
        /// Removes all action clauses from the current scenario.
        /// </summary>
        void ClearActionClauses();

        /// <summary>
        /// Removes all query clauses from the current scenario.
        /// </summary>
        void ClearQueryClauses();

        /// <summary>
        /// Extends the user input action domain with the supplied <see cref="actionDomainInput"/> string.
        /// </summary>
        /// <param name="actionDomainInput">New action domain input.</param>
        void ExtendActionClauses(string actionDomainInput);

        /// <summary>
        /// Extends the collection of action clauses in the current scenario
        /// with members from the supplied <see cref="actionClauses"/> collection
        /// and optional <see cref="actionDomainInput"/> string.
        /// </summary>
        /// <param name="actionClauses">New action clauses collection.</param>
        /// <param name="actionDomainInput">Optional new action domain input.</param>
        void ExtendActionClauses(IEnumerable<IActionClauseViewModel> actionClauses, string actionDomainInput = null);

        /// <summary>
        /// Extends the user input query set with the supplied <see cref="querySetInput"/> string.
        /// </summary>
        /// <param name="querySetInput">New query set input.</param>
        void ExtendQueryClauses(string querySetInput);

        /// <summary>
        /// Extends the collection of query clauses in the current scenario
        /// with members from the supplied <see cref="queryClauses"/> collection
        /// and optional <see cref="querySetInput"/> string.
        /// </summary>
        /// <param name="queryClauses">New query clauses collection.</param>
        /// <param name="querySetInput">Optional new query set input.</param>
        void ExtendQueryClauses(IEnumerable<IQueryClauseViewModel> queryClauses, string querySetInput = null);

        /// <summary>
        /// Retrieves the currently defined scenario along with the language signature.
        /// </summary>
        /// <returns><see cref="Scenario"/> instance which is a full description of currently defined scenario.</returns>
        Scenario GetCurrentScenario();
    }
}