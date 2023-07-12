namespace Reprise
{
    internal sealed class ErrorResponseFactoryTypeProcessor : AbstractTypeProcessor
    {
        private Type? _ErrorResponseFactoryType;

        public override void Process(WebApplicationBuilder builder, Type type)
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

        public override void PostProcess(WebApplicationBuilder builder)
        {
            if (_ErrorResponseFactoryType == null)
            {
                builder.Services.AddSingleton<IErrorResponseFactory, DefaultErrorResponseFactory>();
            }
        }
    }
}
