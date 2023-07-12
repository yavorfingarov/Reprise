namespace Reprise
{
    internal sealed class ProducesProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var producesAttributes = handlerInfo.GetCustomAttributes<ProducesAttribute>();
            foreach (var producesAttribute in producesAttributes)
            {
                builder.Produces(producesAttribute.StatusCode, producesAttribute.ResponseType,
                    producesAttribute.ContentType, producesAttribute.AdditionalContentTypes);
            }
        }
    }
}
