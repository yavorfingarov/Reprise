namespace Reprise.UnitTests.Features.ExceptionHandling
{
    [UsesVerify]
    public class ExceptionHandlerTests
    {
        private readonly Mock<HttpContext> _MockHttpContext = new();

        private readonly Mock<HttpResponse> _MockHttpResponse = new();

        private readonly Mock<IExceptionLogger> _MockExceptionLogger = new();

        private readonly Mock<IErrorResponseFactory> _MockErrorResponseFactory = new();

        private readonly ILoggerProvider _LoggerProvider = LoggerRecording.Start();

        public ExceptionHandlerTests()
        {
            _MockHttpContext.Setup(m => m.Response)
                .Returns(_MockHttpResponse.Object);
        }

        [Fact]
        public void Ctor()
        {
            var serviceProvider = CreateServiceProvider();

            _ = new ExceptionHandler(_LoggerProvider, serviceProvider, _ => Task.CompletedTask);
        }

        [Fact]
        public Task Ctor_NoExceptionLogger()
        {
            var serviceProvider = CreateServiceProvider(skipExceptionLogger: true);

            return Throws(() => new ExceptionHandler(_LoggerProvider, serviceProvider, _ => Task.CompletedTask))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task Ctor_NoErrorResponseFactory()
        {
            var serviceProvider = CreateServiceProvider(skipErrorResponseFactory: true);

            return Throws(() => new ExceptionHandler(_LoggerProvider, serviceProvider, _ => Task.CompletedTask))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task InvokeAsync()
        {
            var serviceProvider = CreateServiceProvider();
            var nextInvoked = false;
            RequestDelegate next = _ =>
            {
                nextInvoked = true;

                return Task.CompletedTask;
            };
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpContext, nextInvoked });
        }

        [Fact]
        public async Task InvokeAsync_BadHttpRequestException()
        {
            var serviceProvider = CreateServiceProvider();
            RequestDelegate next = _ => throw new BadHttpRequestException("Test message");
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpResponse, _MockExceptionLogger, _MockErrorResponseFactory })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task InvokeAsync_ValidationException()
        {
            var serviceProvider = CreateServiceProvider();
            RequestDelegate next = _ => throw new ValidationException("Test message");
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpResponse, _MockExceptionLogger, _MockErrorResponseFactory })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task InvokeAsync_Exception()
        {
            var serviceProvider = CreateServiceProvider();
            RequestDelegate next = _ => throw new Exception("Test message");
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpResponse, _MockExceptionLogger, _MockErrorResponseFactory })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task InvokeAsync_ExceptionResponseHasStarted()
        {
            var serviceProvider = CreateServiceProvider();
            RequestDelegate next = _ => throw new Exception("Test message");
            _MockHttpResponse.Setup(m => m.HasStarted)
                .Returns(true);
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpResponse, _MockExceptionLogger, _MockErrorResponseFactory })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task InvokeAsync_HandleThrows()
        {
            var serviceProvider = CreateServiceProvider();
            RequestDelegate next = _ => throw new Exception("Test message");
            _MockErrorResponseFactory.Setup(m => m.Create(It.IsAny<ErrorContext<Exception>>()))
                .Throws(new NotImplementedException());
            var exceptionHandler = new ExceptionHandler(_LoggerProvider, serviceProvider, next);

            await exceptionHandler.InvokeAsync(_MockHttpContext.Object);

            await Verify(new { _MockHttpResponse, _MockExceptionLogger, _MockErrorResponseFactory })
                .IgnoreStackTrace();
        }

        private IServiceProvider CreateServiceProvider(bool skipExceptionLogger = false, bool skipErrorResponseFactory = false)
        {
            var serviceCollection = new ServiceCollection();
            if (!skipExceptionLogger)
            {
                serviceCollection.AddSingleton(_MockExceptionLogger.Object);
            }
            if (!skipErrorResponseFactory)
            {
                serviceCollection.AddSingleton(_MockErrorResponseFactory.Object);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
