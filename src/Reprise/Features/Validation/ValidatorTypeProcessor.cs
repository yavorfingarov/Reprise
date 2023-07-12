namespace Reprise
{
    internal sealed class ValidatorTypeProcessor : AbstractTypeProcessor
    {
        private readonly Dictionary<Type, Type> _Validators = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            var interfaceTypes = type.GetInterfaces();
            foreach (var interfaceType in interfaceTypes)
            {
                if (interfaceType.IsGenericType)
                {
                    var genericTypeDefinition = interfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == typeof(IValidator<>))
                    {
                        AddValidator(builder.Services, interfaceType, type);

                        break;
                    }
                }
            }
        }

        private void AddValidator(IServiceCollection services, Type interfaceType, Type implementationType)
        {
            if (_Validators.TryGetValue(interfaceType, out var existingType))
            {
                throw new InvalidOperationException(
                    $"{interfaceType.GetGenericArguments()[0]} is validated by both {implementationType} and {existingType}.");
            }
            services.AddSingleton(interfaceType, implementationType);
            _Validators[interfaceType] = implementationType;
        }
    }
}
