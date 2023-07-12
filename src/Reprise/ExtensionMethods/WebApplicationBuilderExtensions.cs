namespace Reprise
{
    /// <summary>
    /// Extension methods for configuring services at application startup.
    /// </summary>
    public static partial class WebApplicationBuilderExtensions
    {
        internal static TypeProcessorFactory TypeProcessorFactory { get; set; } = new();

        /// <summary>
        /// Sets up the DI container by performing an assembly scan to:
        /// <list type="bullet">
        ///     <item>
        ///     Discover all API endpoints (types decorated with <see cref="EndpointAttribute"/>).
        ///     </item>
        ///     <item>
        ///     Call all <see cref="IServiceConfigurator.ConfigureServices(WebApplicationBuilder)"/> 
        ///     implementations.
        ///     </item>
        ///     <item>
        ///     Bind all configurations decorated with <see cref="ConfigurationAttribute"/> 
        ///     and add them with a <see cref="ServiceLifetime.Singleton"/>.
        ///     </item>
        ///     <item>
        ///     Add all <see cref="IValidator{T}"/> implementations with a <see cref="ServiceLifetime.Singleton"/>.
        ///     </item>
        ///     <item>
        ///     Add a <see cref="IExceptionLogger"/> implementation or a default one if none is found.
        ///     </item>
        ///     <item>
        ///     Add a <see cref="IErrorResponseFactory"/> implementation or a default one if none is found.
        ///     </item>
        ///     <item>
        ///     Add all <see cref="IEventHandler{TEvent}"/> implementations with a <see cref="ServiceLifetime.Scoped"/>.
        ///     </item>
        ///     <item>
        ///     Add the <see cref="IEventBus"/>.
        ///     </item>
        ///     <item>
        ///     Add all <see cref="IJob"/> implementations with a <see cref="ServiceLifetime.Scoped"/>.
        ///     </item>
        /// </list>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            return builder.ConfigureServices(Assembly.GetCallingAssembly());
        }

        /// <inheritdoc cref="ConfigureServices(WebApplicationBuilder)"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(assembly);
            var processors = TypeProcessorFactory.CreateAll();
            var types = assembly.GetExportedTypes().Where(t => t.IsClass && !t.IsAbstract);
            foreach (var type in types)
            {
                foreach (var processor in processors)
                {
                    processor.Process(builder, type);
                }
            }
            foreach (var processor in processors)
            {
                processor.PostProcess(builder);
            }

            return builder;
        }
    }

    internal class TypeProcessorFactory
    {
        internal virtual AbstractTypeProcessor[] CreateAll()
        {
            return GetType().Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(AbstractTypeProcessor)))
                .Select(t => (AbstractTypeProcessor)Activator.CreateInstance(t)!)
                .ToArray();
        }
    }
}
