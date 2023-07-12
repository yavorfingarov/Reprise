namespace Reprise
{
    internal sealed class ExceptionLoggerTypeProcessor : AbstractTypeProcessor
    {
        private Type? _ExceptionLoggerType;

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IExceptionLogger)))
            {
                if (_ExceptionLoggerType != null)
                {
                    throw new InvalidOperationException(
                        $"{nameof(IExceptionLogger)} is implemented by both {_ExceptionLoggerType} and {type}.");
                }
                builder.Services.AddSingleton(typeof(IExceptionLogger), type);
                _ExceptionLoggerType = type;
            }
        }

        public override void PostProcess(WebApplicationBuilder builder)
        {
            if (_ExceptionLoggerType == null)
            {
                builder.Services.AddSingleton<IExceptionLogger, DefaultExceptionLogger>();
            }
        }
    }
}
