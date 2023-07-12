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
        internal string SubSectionKey { get; }

        /// <summary>
        /// Creates a new <see cref="ConfigurationAttribute"/>.
        /// </summary>
        public ConfigurationAttribute(string subSectionKey)
        {
            SubSectionKey = subSectionKey;
        }
    }
}
