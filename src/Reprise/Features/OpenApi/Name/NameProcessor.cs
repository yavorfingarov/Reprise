namespace Reprise
{
    internal sealed class NameProcessor : IRouteHandlerBuilderProcessor
    {
        private readonly Dictionary<string, Type> _NamedEndpoints = new();

        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var nameAttribute = handlerInfo.GetCustomAttribute<NameAttribute>();
            if (nameAttribute != null)
            {
                if (nameAttribute.Name.IsEmpty())
                {
                    throw new InvalidOperationException($"{handlerInfo.DeclaringType} has an empty name.");
                }
                if (_NamedEndpoints.TryGetValue(nameAttribute.Name, out var existingType))
                {
                    throw new InvalidOperationException(
                        $"Name '{nameAttribute.Name}' is used by both {handlerInfo.DeclaringType} and {existingType}.");
                }
                builder.WithName(nameAttribute.Name);
                _NamedEndpoints.Add(nameAttribute.Name, handlerInfo.DeclaringType!);
            }
        }
    }
}
