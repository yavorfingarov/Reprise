namespace Reprise
{
    internal sealed class AuthorizationProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            if (options.AuthorizationPolicyNames != null)
            {
                builder.RequireAuthorization(options.AuthorizationPolicyNames);
            }
        }
    }
}
