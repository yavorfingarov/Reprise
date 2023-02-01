namespace Reprise
{
    internal static partial class LoggerExtensions
    {
        private static readonly Action<ILogger, string?, Exception> _MessageHandlerException = LoggerMessage.Define<string?>(
            LogLevel.Error, default,
            "An exception was thrown while executing {EventHandlerType}.");

        public static void LogEventHandlerException(this ILogger logger, Exception exception, string? eventHandlerType)
        {
            _MessageHandlerException(logger, eventHandlerType, exception);
        }
    }
}
