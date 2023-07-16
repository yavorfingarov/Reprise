namespace Reprise
{
    internal sealed class ValidatorTypeProcessor : AbstractTypeProcessor
    {
        private readonly Dictionary<Type, Type> _Validators = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.TryGetGenericInterfaceType(typeof(IValidator<>), out var interfaceType))
            {
                if (_Validators.TryGetValue(interfaceType.GenericTypeArguments[0], out var existingType))
                {
                    throw new InvalidOperationException(
                        $"{interfaceType.GenericTypeArguments[0]} is validated by both {type} and {existingType}.");
                }
                builder.Services.AddSingleton(interfaceType, type);
                _Validators[interfaceType.GenericTypeArguments[0]] = type;
            }
        }
    }
}
