namespace Reprise
{
    internal sealed class ExcludeFromDescriptionProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            if (handlerInfo.IsDefined(typeof(ExcludeFromDescriptionAttribute)))
            {
                builder.ExcludeFromDescription();
            }
        }
    }
}
