namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class ErrorResponseFactoryTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly ErrorResponseFactoryTypeProcessor _Processor = new();

        public ErrorResponseFactoryTypeProcessorTests()
        {
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubErrorResponseFactory));

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
        public Task Process_DuplicateErrorResponseFactory()
        {
            _Processor.Process(_Builder, typeof(StubErrorResponseFactory));

            return Throws(() => _Processor.Process(_Builder, typeof(StubDuplicateErrorResponseFactory)))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.Process(_Builder, typeof(StubErrorResponseFactory));

            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task PostProcess_NoErrorResponseFactory()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    internal class StubErrorResponseFactory : IErrorResponseFactory
    {
        public object? Create(ErrorContext<BadHttpRequestException> context)
        {
            throw new NotImplementedException();
        }

        public object? Create(ErrorContext<ValidationException> context)
        {
            throw new NotImplementedException();
        }

        public object? Create(ErrorContext<Exception> context)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubDuplicateErrorResponseFactory : IErrorResponseFactory
    {
        public object? Create(ErrorContext<BadHttpRequestException> context)
        {
            throw new NotImplementedException();
        }

        public object? Create(ErrorContext<ValidationException> context)
        {
            throw new NotImplementedException();
        }

        public object? Create(ErrorContext<Exception> context)
        {
            throw new NotImplementedException();
        }
    }
}
