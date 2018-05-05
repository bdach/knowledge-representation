using System.Collections.Generic;
using Client.DataTransfer;
using Client.Global;
using Client.Provider;

namespace Client.Interface
{
    /// <summary>
    /// Interface implemented by classes which provider support that requires conversion
    /// of <see cref="Scenario"/> object to a set of corresponding view models.
    /// </summary>
    /// <remarks>
    /// Implemented primarily by <see cref="SerializationProvider"/>.
    /// </remarks>
    public interface IScenarioConverter
    {
        /// <summary>
        /// Retrieves language signature from the given <see cref="scenario"/>.
        /// </summary>
        /// <param name="scenario">Scenario instance to be processed.</param>
        /// <returns><see cref="LanguageSignature"/> instance.</returns>
        LanguageSignature GetLanguageSignature(Scenario scenario);

        /// <summary>
        /// Retrieves action clauses from the given <see cref="scenario"/>.
        /// </summary>
        /// <param name="scenario">Scenario instance to be processed.</param>
        /// <returns>Collection of <see cref="IActionClauseViewModel"/> implementors.</returns>
        IEnumerable<IActionClauseViewModel> GetActionClauseViewModels(Scenario scenario);

        /// <summary>
        /// Retrieves query clauses from the given <see cref="scenario"/>.
        /// </summary>
        /// <param name="scenario">Scenario instance to be processed.</param>
        /// <returns>Collection of <see cref="IQueryClauseViewModel"/> implementors.</returns>
        IEnumerable<IQueryClauseViewModel> GetQueryClauseViewModels(Scenario scenario);
    }
}