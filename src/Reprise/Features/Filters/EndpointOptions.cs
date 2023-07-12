#if NET7_0
namespace Reprise
{
    public partial class EndpointOptions
    {
        internal List<(int Order, Type FilterType)> FilterTypes { get; } = new();

        private int _CurrentFilterOrder;

        /// <summary>
        /// Adds a filter for all API endpoints.
        /// </summary>
        public void AddEndpointFilter<TFilter>(int? order = null) where TFilter : IEndpointFilter
        {
            FilterTypes.Add((order ?? _CurrentFilterOrder++, typeof(TFilter)));
        }

        /// <summary>
        /// Adds a validation filter for all API endpoints.
        /// </summary>
        public void AddValidationFilter(int? order = null)
        {
            FilterTypes.Add((order ?? _CurrentFilterOrder++, typeof(ValidationFilterFactory)));
        }
    }
}
#endif
