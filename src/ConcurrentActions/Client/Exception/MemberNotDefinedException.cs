namespace Client.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Custom exception thrown when a view model is being mapped to its corresponding
    /// model and one of the members is either empty or null.
    /// </summary>
    public class MemberNotDefinedException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="MemberNotDefinedException"/> instance.
        /// </summary>
        public MemberNotDefinedException() { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="MemberNotDefinedException"/> instance
        /// with the supplied exception <see cref="message"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public MemberNotDefinedException(string message) : base(message) { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="MemberNotDefinedException"/> instance
        /// with the supplied exception <see cref="message"/> and <see cref="inner"/> exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public MemberNotDefinedException(string message, System.Exception inner) : base(message, inner) { }
    }
}