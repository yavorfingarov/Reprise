namespace Reprise
{
    internal class AuthorizationProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            if (options._RequireAuthorization)
            {
                builder.RequireAuthorization(options._AuthorizationPolicyNames);
            }
        }
    }
}
