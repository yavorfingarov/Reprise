namespace Reprise
{
    internal sealed class MapperTypeProcessor : AbstractTypeProcessor
    {
        private readonly Dictionary<(Type, Type), Type> _Mappings = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.TryGetGenericInterfaceType(typeof(IMapper<,>), out var interfaceType))
            {
                if (_Mappings.TryGetValue((interfaceType.GenericTypeArguments[0], interfaceType.GenericTypeArguments[1]), out var existingType) ||
                    _Mappings.TryGetValue((interfaceType.GenericTypeArguments[1], interfaceType.GenericTypeArguments[0]), out existingType))
                {
                    throw new InvalidOperationException(
                        $"{interfaceType.GenericTypeArguments[0]} and {interfaceType.GenericTypeArguments[1]} are mapped by both {type} and {existingType}.");
                }
                builder.Services.AddSingleton(interfaceType, type);
                _Mappings[(interfaceType.GenericTypeArguments[0], interfaceType.GenericTypeArguments[1])] = type;
            }
        }
    }
}
