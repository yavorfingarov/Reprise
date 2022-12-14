namespace Reprise
{
    internal static class ExtensionMethods
    {
        internal static object CreateInstance(this Type type)
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

        internal static T GetRequiredServiceSafe<T>(this IServiceProvider services) where T : notnull
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

        internal static string GetFullName(this MethodInfo methodInfo)
        {
            return $"{methodInfo.DeclaringType}.{methodInfo.Name}";
        }

        internal static bool IsEmpty(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }
    }
}
