namespace Reprise
{
    /// <summary>
    /// Specifies a name that is used for link generation and is treated as the operation ID in the OpenAPI description.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NameAttribute : Attribute
    {
        internal string Name { get; }

        /// <summary>
        /// Creates a new <see cref="NameAttribute"/>.
        /// </summary>
        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}
