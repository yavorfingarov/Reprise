using Microsoft.AspNetCore.Routing;

namespace Reprise
{
    /// <summary>
    /// Extension methods for mapping API endpoints.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps all API endpoints discovered by 
        /// <see cref="WebApplicationBuilderExtensions.ConfigureServices(WebApplicationBuilder)"/>
        /// with the default <see cref="EndpointOptions"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
        {
            return app.MapEndpoints(null!);
        }

        /// <summary>
        /// Maps all API endpoints discovered by 
        /// <see cref="WebApplicationBuilderExtensions.ConfigureServices(WebApplicationBuilder)"/>
        /// with custom <see cref="EndpointOptions"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app, Action<EndpointOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(app);
            var options = new EndpointOptions();
            configure?.Invoke(options);
            var processors = typeof(EndpointRouteBuilderExtensions).Assembly.GetTypes()
                .Where(t => t.IsClass && t.IsAssignableTo(typeof(IRouteHandlerBuilderProcessor)))
                .Select(t => (IRouteHandlerBuilderProcessor)Activator.CreateInstance(t)!)
                .ToArray();
            var endpointMapper = app.ServiceProvider.GetInternalDependency<EndpointMapper>();
            endpointMapper.MapEndpoints(app, options, processors);

            return app;
        }
    }
}
