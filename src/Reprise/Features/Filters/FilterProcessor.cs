#if NET7_0
namespace Reprise
{
    internal sealed class FilterProcessor : IRouteHandlerBuilderProcessor
    {
        private readonly MethodInfo _AddEndpointFilterMethod = typeof(EndpointFilterExtensions)
            .GetMethod(nameof(EndpointFilterExtensions.AddEndpointFilter), 1, new[] { typeof(RouteHandlerBuilder) })!;

        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var filterTypes = handlerInfo.GetCustomAttributes<FilterAttribute>()
                .Select(a => (a.Order, a.FilterType))
                .Concat(options.FilterTypes)
                .OrderBy(f => f.Order)
                .Select(f => f.FilterType);
            foreach (var filterType in filterTypes)
            {
                if (filterType == typeof(ValidationFilterFactory))
                {
                    builder.AddEndpointFilterFactory(ValidationFilterFactory.Create);
                }
                else
                {
                    AddFilter(builder, filterType);
                }
            }
        }

        private void AddFilter(RouteHandlerBuilder builder, Type filterType)
        {
            if (!filterType.IsAssignableTo(typeof(IEndpointFilter)))
            {
                throw new InvalidOperationException($"{filterType} does not implement {nameof(IEndpointFilter)}.");
            }
            var addEndpointFilterGenericMethod = _AddEndpointFilterMethod.MakeGenericMethod(filterType);
            addEndpointFilterGenericMethod.Invoke(null, new[] { builder });
        }
    }
}
#endif
