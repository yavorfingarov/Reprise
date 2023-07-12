namespace Reprise.UnitTests.Features.ExceptionHandling
{
    public class DefaultErrorResponseFactoryTests
    {
        private readonly DefaultErrorResponseFactory _DefaultFactory = new();

        [Fact]
        public void Create_BadHttpRequestException()
        {
            var exception = new BadHttpRequestException("Test message");
            var errorContext = new ErrorContext<BadHttpRequestException>(null!, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
        }

        [Fact]
        public void Create_ValidationException()
        {
            var exception = new ValidationException("Test message");
            var errorContext = new ErrorContext<ValidationException>(null!, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
        }

        [Fact]
        public void Create_Exception()
        {
            var exception = new Exception("Test message");
            var errorContext = new ErrorContext<Exception>(null!, exception);

            Assert.Null(_DefaultFactory.Create(errorContext));
        }
    }
}
