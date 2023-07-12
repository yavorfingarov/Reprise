#if NET7_0
namespace Reprise
{
    /// <summary>
    /// Specifies a filter for the API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FilterAttribute : Attribute
    {
        internal int Order { get; }

        internal Type FilterType { get; }

        /// <summary>
        /// Creates a new <see cref="FilterAttribute"/> with default order value.
        /// </summary>
        public FilterAttribute(Type filterType) : this(filterType, int.MaxValue)
        {
        }

        /// <summary>
        /// Creates a new <see cref="FilterAttribute"/> with custom order value.
        /// </summary>
        public FilterAttribute(Type filterType, int order)
        {
            FilterType = filterType;
            Order = order;
        }
    }
}
#endif
