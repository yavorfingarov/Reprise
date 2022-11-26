namespace Reprise
{
    /// <summary>
    /// Specifies a name that is used for link generation and is treated as the operation ID in the OpenAPI description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NameAttribute : Attribute
    {
        internal string _Name;

        /// <summary>
        /// Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        public NameAttribute(string name)
        {
            _Name = name;
        }
    }

    internal sealed class NameProcessor : IRouteHandlerBuilderProcessor
    {
        private readonly Dictionary<string, Type> _NamedEndpoints = new();

        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var nameAttribute = handlerInfo.GetCustomAttribute<NameAttribute>();
            if (nameAttribute != null)
            {
                if (nameAttribute._Name.IsEmpty())
                {
                    throw new InvalidOperationException($"{handlerInfo.DeclaringType} has an empty name.");
                }
                if (_NamedEndpoints.TryGetValue(nameAttribute._Name, out var existingType))
                {
                    throw new InvalidOperationException(
                        $"Name '{nameAttribute._Name}' is used by both {handlerInfo.DeclaringType} and {existingType}.");
                }
                builder.WithName(nameAttribute._Name);
                _NamedEndpoints.Add(nameAttribute._Name, handlerInfo.DeclaringType!);
            }
        }
    }
}
