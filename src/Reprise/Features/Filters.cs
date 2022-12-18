#if NET7_0
namespace Reprise
{
    /// <summary>
    /// Specifies a filter for the API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class FilterAttribute : Attribute
    {
        internal readonly int _Order;

        internal readonly Type _FilterType;

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
            _FilterType = filterType;
            _Order = order;
        }
    }

    public partial class EndpointOptions
    {
        internal readonly List<(int Order, Type FilterType)> _FilterTypes = new();

        private int _CurrentFilterOrder;

        /// <summary>
        /// Adds a filter for all API endpoints.
        /// </summary>
        public void AddEndpointFilter<TFilter>(int? order = null) where TFilter : IEndpointFilter
        {
            _FilterTypes.Add((order ?? _CurrentFilterOrder++, typeof(TFilter)));
        }

        /// <summary>
        /// Adds a validation filter for all API endpoints.
        /// </summary>
        public void AddValidationFilter(int? order = null)
        {
            _FilterTypes.Add((order ?? _CurrentFilterOrder++, typeof(ValidationFilterFactory)));
        }
    }

    internal sealed class FilterProcessor : IRouteHandlerBuilderProcessor
    {
        private readonly MethodInfo _AddEndpointFilterMethod = typeof(EndpointFilterExtensions)
            .GetMethod(nameof(EndpointFilterExtensions.AddEndpointFilter), 1, new[] { typeof(RouteHandlerBuilder) })!;

        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            var filterTypes = handlerInfo.GetCustomAttributes<FilterAttribute>()
                .Select(a => (Order: a._Order, FilterType: a._FilterType))
                .Concat(options._FilterTypes)
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

    internal static class ValidationFilterFactory
    {
        public static EndpointFilterDelegate Create(EndpointFilterFactoryContext factoryContext, EndpointFilterDelegate next)
        {
            var validators = new LinkedList<(int, IValidator)>();
            var parameters = factoryContext.MethodInfo.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(parameters[i].ParameterType);
                var validator = (IValidator?)factoryContext.ApplicationServices.GetService(validatorType);
                if (validator != null)
                {
                    validators.AddLast((i, validator));
                }
            }
            if (validators.Any())
            {
                return CreateEndpointFilterDelegate(validators, next);
            }

            return invocationContext => next(invocationContext);
        }

        private static EndpointFilterDelegate CreateEndpointFilterDelegate(LinkedList<(int, IValidator)> validators, EndpointFilterDelegate next)
        {
            return invocationContext =>
            {
                foreach (var (i, validator) in validators)
                {
                    var dto = invocationContext.Arguments[i];
                    if (dto != null)
                    {
                        var result = validator.Validate(new ValidationContext<object?>(dto));
                        if (!result.IsValid)
                        {
                            throw new ValidationException(result.Errors);
                        }
                    }
                }

                return next(invocationContext);
            };
        }
    }
}
#endif
