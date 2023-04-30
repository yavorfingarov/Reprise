#if NET7_0
namespace Reprise
{
    public partial class EndpointOptions
    {
        internal string? _RateLimitingPolicy;

        /// <summary>
        /// Enables rate limiting for all API endpoints.
        /// </summary>
        public void RequireRateLimiting(string policyName)
        {
            _RateLimitingPolicy = policyName;
        }
    }

    internal sealed class RateLimitingProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            if (options._RateLimitingPolicy != null)
            {
                builder.RequireRateLimiting(options._RateLimitingPolicy);
            }
        }
    }
}
#endif
