namespace Reprise
{
    /// <summary>
    /// Identifies a strongly-typed hierarchical configuration model bound at application startup.
    /// </summary>
    /// <remarks>
    /// A configuration class should be public non-abstract with a public parameterless constructor.
    /// All public read-write properties of the type are bound.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ConfigurationAttribute : Attribute
    {
        internal readonly string _SubSectionKey;

        /// <summary>
        /// Creates a new <see cref="ConfigurationAttribute"/>.
        /// </summary>
        public ConfigurationAttribute(string subSectionKey)
        {
            _SubSectionKey = subSectionKey;
        }
    }

    internal sealed class ConfigurationTypeProcessor : AbstractTypeProcessor
    {
        private readonly Dictionary<string, Type> _Configurations = new();

        internal override void Process(WebApplicationBuilder builder, Type type)
        {
            var configurationAttribute = type.GetCustomAttribute<ConfigurationAttribute>();
            if (configurationAttribute != null)
            {
                AddConfiguration(builder, configurationAttribute._SubSectionKey, type);
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
