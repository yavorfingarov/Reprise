namespace Reprise
{
    /// <summary>
    /// Identifies a type that is an API endpoint.
    /// </summary>
    /// <remarks>
    /// An API endpoint contains a public static <c>Handle</c> method 
    /// that is decorated with a <see cref="GetAttribute"/>, <see cref="PostAttribute"/>,
    /// <see cref="PutAttribute"/>, <see cref="PatchAttribute"/>, <see cref="DeleteAttribute"/>
    /// or <see cref="MapAttribute"/>.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EndpointAttribute : Attribute
    {
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports custom HTTP method(s).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MapAttribute : Attribute
    {
        private protected static readonly string[] _Get = new[] { "GET" };

        private protected static readonly string[] _Post = new[] { "POST" };

        private protected static readonly string[] _Put = new[] { "PUT" };

        private protected static readonly string[] _Patch = new[] { "PATCH" };

        private protected static readonly string[] _Delete = new[] { "DELETE" };

        /// <summary>
        /// Gets the HTTP methods.
        /// </summary>
        public string[] Methods { get; }

        /// <summary>
        /// Gets the route.
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Creates a new <see cref="MapAttribute"/> with a single HTTP method.
        /// </summary>
        public MapAttribute(string method, string route) : this(new[] { method }, route)
        {
        }

        /// <summary>
        /// Creates a new <see cref="MapAttribute"/> with multiple HTTP methods.
        /// </summary>
        public MapAttribute(string[] methods, string route)
        {
            Methods = methods;
            Route = route;
        }
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports the HTTP GET method.
    /// </summary>
    public sealed class GetAttribute : MapAttribute
    {
        /// <summary>
        /// Creates a new <see cref="GetAttribute"/>.
        /// </summary>
        public GetAttribute(string route) : base(_Get, route)
        {
        }
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports the HTTP POST method.
    /// </summary>
    public sealed class PostAttribute : MapAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PostAttribute"/>.
        /// </summary>
        public PostAttribute(string route) : base(_Post, route)
        {
        }
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports the HTTP PUT method.
    /// </summary>
    public sealed class PutAttribute : MapAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PutAttribute"/>.
        /// </summary>
        public PutAttribute(string route) : base(_Put, route)
        {
        }
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports the HTTP PATCH method.
    /// </summary>
    public sealed class PatchAttribute : MapAttribute
    {
        /// <summary>
        /// Creates a new <see cref="PatchAttribute"/>.
        /// </summary>
        public PatchAttribute(string route) : base(_Patch, route)
        {
        }
    }

    /// <summary>
    /// Identifies a public static <c>Handle</c> method that supports the HTTP DELETE method.
    /// </summary>
    public sealed class DeleteAttribute : MapAttribute
    {
        /// <summary>
        /// Creates a new <see cref="DeleteAttribute"/>.
        /// </summary>
        public DeleteAttribute(string route) : base(_Delete, route)
        {
        }
    }
}
