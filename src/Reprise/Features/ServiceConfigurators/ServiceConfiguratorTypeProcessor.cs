namespace Reprise
{
    internal sealed class ServiceConfiguratorTypeProcessor : AbstractTypeProcessor
    {
        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IServiceConfigurator)))
            {
                var configurator = (IServiceConfigurator)type.CreateInstance();
                configurator.ConfigureServices(builder);
            }
        }
    }
}
