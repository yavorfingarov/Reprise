namespace Reprise
{
    /// <summary>
    /// Extension methods for configuring services at application startup.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        internal static TypeProcessorFactory _TypeProcessorFactory = new();

        /// <summary>
        /// Sets up the DI container.
        /// </summary>
        /// <remarks>
        /// This method performs an assembly scan to:
        /// <list type="bullet">
        ///     <item>
        ///     Call all <see cref="IServiceConfigurator.ConfigureServices(WebApplicationBuilder)"/> 
        ///     implementations.
        ///     </item>
        ///     <item>
        ///     Bind all configurations decorated with <see cref="ConfigurationAttribute"/> 
        ///     and add them with a <see cref="ServiceLifetime.Singleton"/>.
        ///     </item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder) =>
            builder.ConfigureServices(Assembly.GetCallingAssembly());

        /// <inheritdoc cref="ConfigureServices(WebApplicationBuilder)"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(assembly);
            var processors = _TypeProcessorFactory.CreateAll();
            var types = assembly.GetExportedTypes().Where(t => t.IsClass && !t.IsAbstract);
            foreach (var type in types)
            {
                processors.ForEach(processor => processor.Process(builder, type));
            }
            processors.ForEach(processor => processor.PostProcess(builder));

            return builder;
        }
    }

    internal class TypeProcessorFactory
    {
        internal virtual List<AbstractTypeProcessor> CreateAll() =>
            typeof(WebApplicationBuilderExtensions).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(AbstractTypeProcessor)))
                .Select(t => (AbstractTypeProcessor)Activator.CreateInstance(t)!)
                .ToList();
    }
}
