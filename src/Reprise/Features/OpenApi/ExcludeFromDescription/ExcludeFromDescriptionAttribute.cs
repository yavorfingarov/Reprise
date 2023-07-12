namespace Reprise
{
    /// <summary>
    /// Specifies an API endpoint that is excluded from the OpenAPI description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExcludeFromDescriptionAttribute : Attribute
    {
    }
}
