namespace Reprise
{
    internal static partial class LoggerExtensions
    {
        private static readonly Action<ILogger, Exception> _ResponseHasStarted = LoggerMessage.Define(
            LogLevel.Error, default,
            "The response has already started, the exception handler will not be executed.");

        private static readonly Action<ILogger, Exception> _ExceptionInHandler = LoggerMessage.Define(
            LogLevel.Error, default,
            "An exception was thrown while executing the exception handler.");

        private static readonly Action<ILogger, string, Exception?> _BadHttpRequestException = LoggerMessage.Define<string>(
            LogLevel.Error, default,
            "BadHttpRequestException: {Message}");

        private static readonly Action<ILogger, Exception> _Exception = LoggerMessage.Define(
            LogLevel.Error, default,
            "An exception was thrown while executing the request.");

        public static void LogResponseHasStarted(this ILogger logger, Exception exception)
        {
            _ResponseHasStarted(logger, exception);
        }

        public static void LogExceptionInHandler(this ILogger logger, Exception exception)
        {
            _ExceptionInHandler(logger, exception);
        }

        public static void LogBadHttpRequestException(this ILogger logger, string message)
        {
            _BadHttpRequestException(logger, message, null);
        }

        public static void LogException(this ILogger logger, Exception exception)
        {
            _Exception(logger, exception);
        }
    }
}
