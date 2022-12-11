namespace Reprise.SampleApi.ErrorHandling
{
    public class ErrorResponseFactory : IErrorResponseFactory
    {
        public object? Create(ErrorContext<BadHttpRequestException> context)
        {
            return new ErrorResponse(context.Exception.Message);
        }

        public object? Create(ErrorContext<ValidationException> context)
        {
            return new ErrorResponse("Validation failed",
                context.Exception.Errors.ToDictionary(f => f.PropertyName, f => f.ErrorMessage));
        }

        public object? Create(ErrorContext<Exception> context)
        {
            return new ErrorResponse(context.Exception.Message);
        }
    }

    public record ErrorResponse(string Message, object? Details = null);
}
