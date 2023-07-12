namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class DefaultExceptionLoggerTests
    {
        private readonly DefaultExceptionLogger _DefaultExceptionLogger = new();

        [Fact]
        public Task Log_BadHttpRequestException()
        {
            var logger = LoggerRecording.Start();
            var exception = new BadHttpRequestException("Test message");
            var errorContext = new ErrorContext<BadHttpRequestException>(null!, exception);

            _DefaultExceptionLogger.Log(logger, errorContext);

            return Verify(logger);
        }

        [Fact]
        public Task Log_ValidationException()
        {
            var logger = LoggerRecording.Start();
            var exception = new ValidationException("Test message");
            var errorContext = new ErrorContext<ValidationException>(null!, exception);

            _DefaultExceptionLogger.Log(logger, errorContext);

            return Verify(logger);
        }

        [Fact]
        public Task Log_Exception()
        {
            var logger = LoggerRecording.Start();
            var exception = new Exception("Test message");
            var errorContext = new ErrorContext<Exception>(null!, exception);

            _DefaultExceptionLogger.Log(logger, errorContext);

            return Verify(logger);
        }
    }
}
