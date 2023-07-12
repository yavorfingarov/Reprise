namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class ExceptionLoggerTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly ExceptionLoggerTypeProcessor _Processor = new();

        public ExceptionLoggerTypeProcessorTests()
        {
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubExceptionLogger));

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Processor.Process(_Builder, GetType());

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_DuplicateExceptionLogger()
        {
            _Processor.Process(_Builder, typeof(StubExceptionLogger));

            return Throws(() => _Processor.Process(_Builder, typeof(StubDuplicateExceptionLogger)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.Process(_Builder, typeof(StubExceptionLogger));

            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task PostProcess_NoExceptionLogger()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    internal class StubExceptionLogger : IExceptionLogger
    {
        public void Log(ILogger logger, ErrorContext<BadHttpRequestException> context)
        {
            throw new NotImplementedException();
        }

        public void Log(ILogger logger, ErrorContext<ValidationException> context)
        {
            throw new NotImplementedException();
        }

        public void Log(ILogger logger, ErrorContext<Exception> context)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubDuplicateExceptionLogger : IExceptionLogger
    {
        public void Log(ILogger logger, ErrorContext<BadHttpRequestException> context)
        {
            throw new NotImplementedException();
        }

        public void Log(ILogger logger, ErrorContext<ValidationException> context)
        {
            throw new NotImplementedException();
        }

        public void Log(ILogger logger, ErrorContext<Exception> context)
        {
            throw new NotImplementedException();
        }
    }
}
