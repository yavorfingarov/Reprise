namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class ErrorResponseFactory
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly HttpContext _HttpContext = new DefaultHttpContext();

        private readonly ErrorResponseFactoryTypeProcessor _Processor = new();

        private readonly DefaultErrorResponseFactory _DefaultFactory = new();

        public ErrorResponseFactory()
        {
            _Builder.Services.Clear();
        }

        [Fact]
        public void DefaultCreate_BadHttpRequestException()
        {
            var exception = new BadHttpRequestException("Test message");
            var errorContext = new ErrorContext<BadHttpRequestException>(_HttpContext, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
        }

        [Fact]
        public void DefaultCreate_ValidationException()
        {
            var exception = new ValidationException("Test message");
            var errorContext = new ErrorContext<ValidationException>(_HttpContext, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
        }

        [Fact]
        public void DefaultCreate_Exception()
        {
            var exception = new Exception("Test message");
            var errorContext = new ErrorContext<Exception>(_HttpContext, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
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
