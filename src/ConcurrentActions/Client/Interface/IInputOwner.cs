using Client.ViewModel;

namespace Client.Interface
{
    /// <summary>
    /// Interface implemented by the root <see cref="ShellViewModel"/>
    /// which allows for grammar and scenario management
    /// </summary>
    public interface IInputOwner : IScenarioOwner, IGrammarOwner
    {
        /// <summary>
        /// Property allows to get or set input mode.
        /// implemented by the root <see cref="ShellViewModel"/>
        /// </summary>
        bool GrammarMode { get; set; }
    }
}