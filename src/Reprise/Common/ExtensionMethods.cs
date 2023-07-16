using System.Diagnostics.CodeAnalysis;

namespace Reprise
{
    internal static class ExtensionMethods
    {
        public static object CreateInstance(this Type type)
        {
            object? instance;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"{type} could not be instantiated. Check the inner exception for more details.", ex);
            }
            if (instance == null)
            {
                throw new InvalidOperationException($"{type} could not be instantiated.");
            }

            return instance;
        }

        public static T GetInternalDependency<T>(this IServiceProvider services) where T : notnull
        {
            try
            {
                return services.GetRequiredService<T>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Reprise could not resolve an internal dependency. Have you called 'builder.ConfigureServices()'?", ex);
            }
        }

        public static string GetFullName(this MethodInfo methodInfo)
        {
            return $"{methodInfo.DeclaringType}.{methodInfo.Name}";
        }

        public static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        public static bool TryGetGenericInterfaceType(this Type type, Type unboundGenericType, [NotNullWhen(true)] out Type? interfaceType)
        {
            var implementedInterfaceTypes = type.GetInterfaces();
            foreach (var implementedInterfaceType in implementedInterfaceTypes)
            {
                if (implementedInterfaceType.IsGenericType)
                {
                    var genericTypeDefinition = implementedInterfaceType.GetGenericTypeDefinition();
                    if (genericTypeDefinition == unboundGenericType)
                    {
                        interfaceType = implementedInterfaceType;

                        return true;
                    }
                }
            }
            interfaceType = null;

            return false;
        }
    }
}
