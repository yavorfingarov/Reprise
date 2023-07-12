namespace Reprise
{
    /// <summary>
    /// Specifies the contract for exception logging.
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// Logs a <see cref="BadHttpRequestException"/>.
        /// </summary>
        void Log(ILogger logger, ErrorContext<BadHttpRequestException> context);

        /// <summary>
        /// Logs a <see cref="ValidationException"/>.
        /// </summary>
        void Log(ILogger logger, ErrorContext<ValidationException> context);

        /// <summary>
        /// Logs an <see cref="Exception"/>.
        /// </summary>
        void Log(ILogger logger, ErrorContext<Exception> context);
    }

    /// <summary>
    /// Specifies the contract for creating error responses.
    /// </summary>
    public interface IErrorResponseFactory
    {
        /// <summary>
        /// Creates a response body when handling a <see cref="BadHttpRequestException"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<BadHttpRequestException> context);

        /// <summary>
        /// Creates a response body when handling a <see cref="ValidationException"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<ValidationException> context);

        /// <summary>
        /// Creates a response when handling an <see cref="Exception"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<Exception> context);
    }
}
