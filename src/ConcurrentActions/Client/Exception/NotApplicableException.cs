namespace Client.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Custom exception thrown when an attempt of adding a formula to a formula-less clause
    /// or adding a program to a program-less clause was made.
    /// </summary>
    // TODO : possibly redundant, remove if not used
    public class NotApplicableException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="NotApplicableException"/> instance.
        /// </summary>
        public NotApplicableException() { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="NotApplicableException"/> instance
        /// with the supplied exception <see cref="message"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NotApplicableException(string message) : base(message) { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="NotApplicableException"/> instance
        /// with the supplied exception <see cref="message"/> and <see cref="inner"/> exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public NotApplicableException(string message, System.Exception inner) : base(message, inner) { }
    }
}