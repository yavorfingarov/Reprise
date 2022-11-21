using System.Diagnostics;

namespace Reprise
{
    /// <summary>
    /// Encapsulates the exception context.
    /// </summary>
    public sealed class ErrorContext<TException> where TException : Exception
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        public TException Exception { get; }

        /// <summary>
        /// Gets the <see cref="HttpContext.Request"/>.
        /// </summary>
        public HttpRequest Request => _HttpContext.Request;

        /// <summary>
        /// Gets the <see cref="HttpContext.TraceIdentifier"/>.
        /// </summary>
        public string TraceIdentifier => _HttpContext.TraceIdentifier;

        /// <summary>
        /// Gets the current <see cref="Activity.Id"/>.
        /// </summary>
        public string? ActivityId => Activity.Current?.Id;

        private readonly HttpContext _HttpContext;

        internal ErrorContext(HttpContext httpContext, TException exception)
        {
            _HttpContext = httpContext;
            Exception = exception;
        }
    }
}
