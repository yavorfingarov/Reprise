#if NET7_0
namespace Reprise.SampleApi.Filters
{
    public class GreetingFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            context.HttpContext.Response.Headers.Add("greeting", "Hello!");

            return next(context);
        }
    }
}
#endif
