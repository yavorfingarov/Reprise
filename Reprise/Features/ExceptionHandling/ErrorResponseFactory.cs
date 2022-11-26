namespace Reprise
{
    /// <summary>
    /// Specifies the contract for creating error responses.
    /// </summary>
    public interface IErrorResponseFactory
    {
        /// <summary>
        /// Creates a response body when handling a <see cref="BadHttpRequestException"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<BadHttpRequestException> context);

        /// <summary>
        /// Creates a response body when handling a <see cref="ValidationException"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<ValidationException> context);

        /// <summary>
        /// Creates a response when handling an <see cref="Exception"/>. 
        /// Return <see langword="null"/> to omit the body.
        /// </summary>
        object? Create(ErrorContext<Exception> context);
    }

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

    internal sealed class ErrorResponseFactoryTypeProcessor : AbstractTypeProcessor
    {
        private Type? _ErrorResponseFactoryType;

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IErrorResponseFactory)))
            {
                if (_ErrorResponseFactoryType != null)
                {
                    throw new InvalidOperationException(
                        $"{nameof(IErrorResponseFactory)} is implemented by both {_ErrorResponseFactoryType} and {type}.");
                }
                builder.Services.AddSingleton(typeof(IErrorResponseFactory), type);
                _ErrorResponseFactoryType = type;
            }
        }

        internal override void PostProcess(WebApplicationBuilder builder)
        {
            if (_ErrorResponseFactoryType == null)
            {
                builder.Services.AddSingleton(typeof(IErrorResponseFactory), typeof(DefaultErrorResponseFactory));
            }
        }
    }
}
