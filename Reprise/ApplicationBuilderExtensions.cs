namespace Reprise
{
    /// <summary>
    /// Extension methods for adding custom middleware.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Reprise exception handler. Its behavior can be customized by implementing 
        /// <see cref="IExceptionLogger"/> and/or <see cref="IErrorResponseFactory"/>.
        /// </summary>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandler>();
        }
    }
}
