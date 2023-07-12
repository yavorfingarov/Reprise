namespace Reprise
{
    internal static partial class LoggerExtensions
    {
        private static readonly Action<ILogger, string?, Exception> _ScheduledJobException = LoggerMessage.Define<string?>(
            LogLevel.Error, default,
            "An exception was thrown while executing {ScheduledJobType}.");

        public static void LogScheduledJobException(this ILogger logger, Exception exception, string? scheduledJobType)
        {
            _ScheduledJobException(logger, scheduledJobType, exception);
        }
    }
}
