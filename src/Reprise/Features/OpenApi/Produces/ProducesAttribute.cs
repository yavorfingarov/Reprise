namespace Reprise
{
    /// <summary>
    /// Describes a response returned from an API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ProducesAttribute : Attribute
    {
        internal int StatusCode { get; }

        internal Type? ResponseType { get; }

        internal string? ContentType { get; }

        internal string[] AdditionalContentTypes { get; }

        /// <summary>
        /// Creates a new <see cref="ProducesAttribute"/>.
        /// </summary>
        public ProducesAttribute(int statusCode,
            Type? responseType = null,
            string? contentType = null,
            params string[] additionalContentTypes)
        {
            StatusCode = statusCode;
            ResponseType = responseType;
            ContentType = contentType;
            AdditionalContentTypes = additionalContentTypes;
        }
    }
}
