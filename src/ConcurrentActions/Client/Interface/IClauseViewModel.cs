namespace Client.Interface
{
    /// <summary>
    /// Base interface for clause view models.
    /// </summary>
    public interface IClauseViewModel
    {
        /// <summary>
        /// The localization key for this clause type.
        /// Used for ribbon grouping.
        /// </summary>
        string ClauseTypeNameKey { get; }
    }
}