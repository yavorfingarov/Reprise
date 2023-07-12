namespace Reprise
{
    internal sealed class DefaultExceptionLogger : IExceptionLogger
    {
        public void Log(ILogger logger, ErrorContext<BadHttpRequestException> context)
        {
            logger.LogBadHttpRequestException(context.Exception.Message);
        }

        public void Log(ILogger logger, ErrorContext<ValidationException> context)
        {
        }

        public void Log(ILogger logger, ErrorContext<Exception> context)
        {
            logger.LogException(context.Exception);
        }
    }
}
