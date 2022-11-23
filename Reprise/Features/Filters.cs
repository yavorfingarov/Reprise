#if NET7_0
namespace Reprise
{
    /// <summary>
    /// Specifies a filter for the API endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FilterAttribute : Attribute
    {
        /// <summary>
        /// Gets the filter type.
        /// </summary>
        public Type FilterType { get; }

        /// <summary>
        /// Creates a new <see cref="FilterAttribute"/>.
        /// </summary>
        public FilterAttribute(Type filterType)
        {
            FilterType = filterType;
        }
    }

    public partial class EndpointOptions
    {
        internal List<Type> _FilterTypes = new();

        /// <summary>
        /// Adds a filter for all API endpoints.
        /// </summary>
        public void AddEndpointFilter<TFilter>() where TFilter : IEndpointFilter
        {
            _FilterTypes.Add(typeof(TFilter));
        }

        /// <summary>
        /// Adds a validation filter for all API endpoints.
        /// </summary>
        public void AddValidationFilter()
        {
            _FilterTypes.Add(typeof(ValidationFilterFactory));
        }
    }

    internal class FilterProcessor : IRouteHandlerBuilderProcessor
    {
        public void Process(RouteHandlerBuilder builder, MethodInfo handlerInfo, EndpointOptions options, string route)
        {
            foreach (var filterType in options._FilterTypes)
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
            var filterAttribute = handlerInfo.GetCustomAttribute<FilterAttribute>();
            if (filterAttribute != null)
            {
                if (!filterAttribute.FilterType.IsAssignableTo(typeof(IEndpointFilter)))
                {
                    throw new InvalidOperationException($"{filterAttribute.FilterType} does not implement {nameof(IEndpointFilter)}.");
                }
                AddFilter(builder, filterAttribute.FilterType);
            }
        }

        private void AddFilter(RouteHandlerBuilder builder, Type filterType)
        {
            var addEndpointFilterOpenMethod = typeof(EndpointFilterExtensions)
                .GetMethod(nameof(EndpointFilterExtensions.AddEndpointFilter), 1, new[] { typeof(RouteHandlerBuilder) });
            var addEndpointFilterClosedMethod = addEndpointFilterOpenMethod!.MakeGenericMethod(filterType);
            addEndpointFilterClosedMethod.Invoke(null, new[] { builder });
        }
    }

    internal static class ValidationFilterFactory
    {
        public static EndpointFilterDelegate Create(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
        {
            var parameters = context.MethodInfo.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var nullableAttribute = parameters[i].CustomAttributes
                    .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
                if (nullableAttribute != null)
                {
                    continue;
                }
                var validatorType = typeof(IValidator<>).MakeGenericType(parameters[i].ParameterType);
                var validator = (IValidator?)context.ApplicationServices.GetService(validatorType);
                if (validator == null)
                {
                    continue;
                }
                return invocationContext =>
                {
                    var result = validator.Validate(new ValidationContext<object?>(invocationContext.Arguments[i]));
                    if (!result.IsValid)
                    {
                        throw new ValidationException(result.Errors);
                    }

                    return next(invocationContext);
                };
            }

            return context => next(context);
        }
    }
}
#endif
