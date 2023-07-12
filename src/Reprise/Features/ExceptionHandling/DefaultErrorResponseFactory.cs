namespace Reprise
{
    internal sealed class DefaultErrorResponseFactory : IErrorResponseFactory
    {
        public object? Create(ErrorContext<BadHttpRequestException> context)
        {
            return null;
        }

        public object? Create(ErrorContext<ValidationException> context)
        {
            return null;
        }

        public object? Create(ErrorContext<Exception> context)
        {
            return null;
        }
    }
}
