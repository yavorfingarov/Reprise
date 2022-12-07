namespace Reprise
{
    public partial class EndpointOptions
    {
        internal bool _RequireAuthorization;

        internal string[] _AuthorizationPolicyNames = Array.Empty<string>();

        /// <summary>
        /// Enables authorization for all API endpoints.
        /// </summary>
        public void RequireAuthorization(params string[] policyNames)
        {
            _RequireAuthorization = true;
            _AuthorizationPolicyNames = policyNames;
        }
    }

    internal sealed class AuthorizationProcessor : IRouteHandlerBuilderProcessor
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
