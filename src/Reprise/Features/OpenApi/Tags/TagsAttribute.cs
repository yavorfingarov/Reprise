namespace Reprise
{
    /// <summary>
    /// Specifies custom OpenAPI tags for the API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TagsAttribute : Attribute
    {
        internal string[] Tags { get; }

        /// <summary>
        /// Creates a new <see cref="TagsAttribute"/>.
        /// </summary>
        public TagsAttribute(string tag, params string[] additionalTags)
        {
            Tags = additionalTags.Prepend(tag).ToArray();
        }
    }
}
