namespace Reprise
{
    /// <summary>
    /// Specifies an API endpoint that is excluded from the OpenAPI description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExcludeFromDescriptionAttribute : Attribute
    {
    }

    internal sealed class ExcludeFromDescriptionProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var excludeFromDescriptionAttribute = handlerInfo.GetCustomAttribute<ExcludeFromDescriptionAttribute>();
            if (excludeFromDescriptionAttribute != null)
            {
                builder.ExcludeFromDescription();
            }
        }
    }
}
