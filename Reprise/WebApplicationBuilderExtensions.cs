namespace Reprise
{
    /// <summary>
    /// Extension methods for configuring services at application startup.
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        /// <summary>
        /// Calls all <see cref="IServiceConfigurator.ConfigureServices(WebApplicationBuilder)"/> implementations in the current assembly.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            return builder.ConfigureServices(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Calls all <see cref="IServiceConfigurator.ConfigureServices(WebApplicationBuilder)"/> implementations in the specified assembly.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IServiceConfigurator)));
            foreach (var type in types)
            {
                var confgurator = CreateConfigurator(type);
                confgurator?.ConfigureServices(builder);
            }

            return builder;
        }

        internal static IServiceConfigurator? CreateConfigurator(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new InvalidOperationException($"{type} has no public parameterless contructor.");
            }
            var confgurator = (IServiceConfigurator?)Activator.CreateInstance(type);

            return confgurator;
        }
    }
}
