#if NET7_0
namespace Reprise
{
    internal sealed class RateLimitingProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            if (options.RateLimitingPolicy != null)
            {
                builder.RequireRateLimiting(options.RateLimitingPolicy);
            }
        }
    }
}
#endif
