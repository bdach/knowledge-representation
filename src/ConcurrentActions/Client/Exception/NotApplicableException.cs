namespace Client.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Custom exception thrown when an attempt of adding a formula to a formula-less clause
    /// or adding a program to a program-less clause was made.
    /// </summary>
    public class NotApplicableException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NotApplicableException() { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with exception message.
        /// </summary>
        /// <param name="message">exception message</param>
        public NotApplicableException(string message) : base(message) { }

        /// <inheritdoc />
        /// <summary>
        /// Constructor with exception message and inner exception.
        /// </summary>
        /// <param name="message">exception message</param>
        /// <param name="inner">enner exception</param>
        public NotApplicableException(string message, System.Exception inner) : base(message, inner) { }
    }
}