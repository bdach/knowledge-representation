namespace Client.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Custom exception thrown when a view model is being mapped to its corresponding
    /// model and one of the members is either empty or null.
    /// </summary>
    public class MissingMemberException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MissingMemberException() { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with exception message.
        /// </summary>
        /// <param name="message">exception message</param>
        public MissingMemberException(string message) : base(message) { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with exception message and inner exception.
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="inner">enner exception</param>
        public MissingMemberException(string message, System.Exception inner) : base(message, inner) { }
    }
}