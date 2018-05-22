using Client.DataTransfer;
using Client.ViewModel;

namespace Client.Interface
{
    /// <summary>
    /// Interface implemented by the root <see cref="ShellViewModel"/>
    /// which allows for grammar management
    /// </summary>
    public interface IGrammarOwner
    {
        /// <summary>
        /// Removes the user input action domain.
        /// </summary>
        void ClearActionInput();

        /// <summary>
        /// Removes the user input query set.
        /// </summary>
        void ClearQueryInput();

        /// <summary>
        /// Extends the user input action domain with the supplied <see cref="actionDomainInput"/> string.
        /// </summary>
        /// <param name="actionDomainInput">New action domain input.</param>
        void ExtendActionInput(string actionDomainInput);

        /// <summary>
        /// Extends the user input query set with the supplied <see cref="querySetInput"/> string.
        /// </summary>
        /// <param name="querySetInput">New query set input.</param>
        void ExtendQueryInput(string querySetInput);

        /// <summary>
        /// Retrieves the currently defined grammar.
        /// </summary>
        /// <returns><see cref="GrammarInput"/> instance which is a full description of currently defined grammar.</returns>
        GrammarInput GetCurrentGrammar();
    }
}