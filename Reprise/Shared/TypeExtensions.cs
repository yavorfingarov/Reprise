namespace Reprise
{
    internal static class TypeExtensions
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
                throw new InvalidOperationException($"{type} could not be instantiated. Check the inner exception for more details.", ex);
            }
            if (instance == null)
            {
                throw new InvalidOperationException($"{type} could not be instantiated.");
            }

            return instance;
        }
    }
}
