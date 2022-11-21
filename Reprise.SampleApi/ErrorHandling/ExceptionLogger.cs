namespace Reprise.SampleApi.ErrorHandling
{
    public class ExceptionLogger : IExceptionLogger
    {
        public void Log(ILogger logger, ErrorContext<BadHttpRequestException> context)
        {
            logger.LogError("BadHttpRequestException: {Message} ({HttpMethod} {Route})",
                context.Exception.Message, context.Request.Method, context.Request.Path);
        }

        public void Log(ILogger logger, ErrorContext<ValidationException> context)
        {
            logger.LogError("ValidationException ({HttpMethod} {Route})",
                context.Request.Method, context.Request.Path);
        }

        public void Log(ILogger logger, ErrorContext<Exception> context)
        {
            logger.LogError(context.Exception, "An unhandled exception has occurred while executing the request.");
        }
    }
}
