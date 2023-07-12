#if NET7_0
namespace Reprise
{
    internal static class ValidationFilterFactory
    {
        public static EndpointFilterDelegate Create(EndpointFilterFactoryContext factoryContext, EndpointFilterDelegate next)
        {
            var validators = new List<(int, IValidator)>();
            var parameters = factoryContext.MethodInfo.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var validatorType = typeof(IValidator<>).MakeGenericType(parameters[i].ParameterType);
                var validator = (IValidator?)factoryContext.ApplicationServices.GetService(validatorType);
                if (validator != null)
                {
                    validators.Add((i, validator));
                }
            }
            if (validators.Any())
            {
                return CreateEndpointFilterDelegate(validators, next);
            }

            return invocationContext => next(invocationContext);
        }

        private static EndpointFilterDelegate CreateEndpointFilterDelegate(List<(int, IValidator)> validators, EndpointFilterDelegate next)
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
