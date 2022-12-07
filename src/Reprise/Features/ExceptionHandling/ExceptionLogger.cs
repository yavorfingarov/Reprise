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

    internal sealed class DefaultExceptionLogger : IExceptionLogger
    {
        public void Log(ILogger logger, ErrorContext<BadHttpRequestException> context)
        {
            logger.LogError("BadHttpRequestException: {Message}", context.Exception.Message);
        }

        public void Log(ILogger logger, ErrorContext<ValidationException> context)
        {
        }

        public void Log(ILogger logger, ErrorContext<Exception> context)
        {
            logger.LogError(context.Exception, "An exception was thrown while executing the request.");
        }
    }

    internal sealed class ExceptionLoggerTypeProcessor : AbstractTypeProcessor
    {
        private Type? _ExceptionLoggerType;

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IExceptionLogger)))
            {
                if (_ExceptionLoggerType != null)
                {
                    throw new InvalidOperationException(
                        $"{nameof(IExceptionLogger)} is implemented by both {_ExceptionLoggerType} and {type}.");
                }
                builder.Services.AddSingleton(typeof(IExceptionLogger), type);
                _ExceptionLoggerType = type;
            }
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            if (_ExceptionLoggerType == null)
            {
                builder.Services.AddSingleton(typeof(IExceptionLogger), typeof(DefaultExceptionLogger));
            }
        }
    }
}
