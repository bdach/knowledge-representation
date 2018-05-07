namespace Client.Exception
{
    /// <inheritdoc />
    /// <summary>
    /// Custom exception thrown when a serialization or deserialization process fails.
    /// </summary>
    public class SerializationException : System.Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="SerializationException"/> instance.
        /// </summary>
        public SerializationException() { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="SerializationException"/> instance
        /// with the supplied exception <see cref="message"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public SerializationException(string message) : base(message) { }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new <see cref="SerializationException"/> instance
        /// with the supplied exception <see cref="message"/> and <see cref="inner"/> exception.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public SerializationException(string message, System.Exception inner) : base(message, inner) { }
    }
}