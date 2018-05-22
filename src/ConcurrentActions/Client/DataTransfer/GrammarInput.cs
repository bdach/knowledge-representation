namespace Client.DataTransfer
{
    /// <summary>
    /// Container holding user inputs from grammar tab
    /// </summary>
    public class GrammarInput
    {
        /// <summary>
        /// Manually entered action domain string.
        /// </summary>
        public string ActionDomainInput { get; set; }

        /// <summary>
        /// Manually entered query set string.
        /// </summary>
        public string QuerySetInput { get; set; }
    }
}