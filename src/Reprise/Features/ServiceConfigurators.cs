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

    internal sealed class ServiceConfiguratorTypeProcessor : AbstractTypeProcessor
    {
        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IServiceConfigurator)))
            {
                var confgurator = (IServiceConfigurator)type.CreateInstance();
                confgurator.ConfigureServices(builder);
            }
        }
    }
}
