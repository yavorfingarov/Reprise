namespace Reprise
{
    /// <summary>
    /// Specifies the contract to configure services at application startup.
    /// </summary>
    public interface IServiceConfigurator
    {
        /// <summary>
        /// Configures services for the specified <see cref="WebApplicationBuilder"/>.
        /// </summary>
        void ConfigureServices(WebApplicationBuilder builder);
    }
}
