namespace Reprise
{
    /// <summary>
    /// Describes a response returned from an API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ProducesAttribute : Attribute
    {
        internal readonly int _StatusCode;

        internal readonly Type? _ResponseType;

        internal readonly string? _ContentType;

        internal readonly string[] _AdditionalContentTypes;

        /// <summary>
        /// Creates a new <see cref="ProducesAttribute"/>.
        /// </summary>
        public ProducesAttribute(int statusCode,
            Type? responseType = null,
            string? contentType = null,
            params string[] additionalContentTypes)
        {
            _StatusCode = statusCode;
            _ResponseType = responseType;
            _ContentType = contentType;
            _AdditionalContentTypes = additionalContentTypes;
        }
    }

    internal sealed class ProducesProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var producesAttributes = handlerInfo.GetCustomAttributes<ProducesAttribute>();
            foreach (var producesAttribute in producesAttributes)
            {
                builder.Produces(producesAttribute._StatusCode, producesAttribute._ResponseType,
                    producesAttribute._ContentType, producesAttribute._AdditionalContentTypes);
            }
        }
    }
}
