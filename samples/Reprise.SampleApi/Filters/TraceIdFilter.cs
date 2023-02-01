#if NET7_0
namespace Reprise.SampleApi.Filters
{
    public class TraceIdFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            context.HttpContext.Response.Headers.Add("trace-id", context.HttpContext.TraceIdentifier);

            return next(context);
        }
    }
}
#endif
