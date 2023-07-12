namespace Reprise
{
    internal sealed class ConfigurationTypeProcessor : AbstractTypeProcessor
    {
        private readonly Dictionary<string, Type> _Configurations = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            var configurationAttribute = type.GetCustomAttribute<ConfigurationAttribute>();
            if (configurationAttribute != null)
            {
                AddConfiguration(builder, configurationAttribute.SubSectionKey, type);
            }
        }

        private void AddConfiguration(WebApplicationBuilder builder, string subSectionKey, Type type)
        {
            if (subSectionKey.IsEmpty())
            {
                throw new InvalidOperationException($"{type} has an empty sub-section key.");
            }
            if (_Configurations.TryGetValue(subSectionKey, out var existingType))
            {
                throw new InvalidOperationException($"Sub-section key '{subSectionKey}' is bound to both {type} and {existingType}.");
            }
            var configurationSection = builder.Configuration.GetSection(subSectionKey);
            if (!configurationSection.Exists())
            {
                throw new InvalidOperationException($"{type} has a sub-section key that doesn't match any configuration data.");
            }
            var configuration = type.CreateInstance();
            configurationSection.Bind(configuration, options => options.ErrorOnUnknownConfiguration = true);
            builder.Services.AddSingleton(configuration.GetType(), configuration);
            _Configurations[subSectionKey] = type;
        }
    }
}
