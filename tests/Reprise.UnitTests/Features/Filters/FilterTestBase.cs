#if NET7_0
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features.Filters
{
    [UsesVerify]
    public abstract class FilterTestBase : IDisposable
    {
        internal IHost? Host { get; set; }

        internal HttpClient Client { get; set; } = null!;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Host?.Dispose();
        }

        internal async Task SetupHost(Type endpointType, Action<EndpointOptions>? configureOptions = null)
        {
            var endpointMapper = new EndpointMapper();
            endpointMapper.EndpointTypes?.Add(endpointType);
            Host = await new HostBuilder()
                .ConfigureWebHost(builder =>
                {
                    builder.UseTestServer();
                    builder.ConfigureServices(services =>
                    {
                        services.AddRouting();
                        services.AddSingleton(endpointMapper);
                        services.AddSingleton<IExceptionLogger, DefaultExceptionLogger>();
                        services.AddSingleton<IErrorResponseFactory, StubErrorResponseFactory>();
                        services.AddSingleton<IValidator<StubDto>, StubDtoValidator>();
                        services.AddSingleton<IValidator<StubQueryValues>, StubQueryValuesValidator>();
                    });
                    builder.Configure(app =>
                    {
                        app.UseExceptionHandling();
                        app.UseRouting();
                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapEndpoints(configureOptions!);
                        });
                    });
                })
                .StartAsync();
            Client = Host.GetTestClient();
        }
    }

    internal abstract class AbstractStubFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var filterInvocationsHeader = context.HttpContext.Response.Headers["filter-invocations"].ToString();
            filterInvocationsHeader += filterInvocationsHeader.IsEmpty() ? GetType().Name : $" / {GetType().Name}";
            context.HttpContext.Response.Headers["filter-invocations"] = filterInvocationsHeader;

            return next(context);
        }
    }

    internal class StubFilterA : AbstractStubFilter
    {
    }

    internal class StubFilterB : AbstractStubFilter
    {
    }

    internal class StubFilterC : AbstractStubFilter
    {
    }

    internal class StubFilterD : AbstractStubFilter
    {
    }

    internal class StubFilterNoImplementation
    {
    }

    internal class StubEndpointWithoutFilter
    {
        [Get("/")]
        public static string Handle()
        {
            return "Hello, world!";
        }
    }

    internal class StubEndpointWithFilter
    {
        [Get("/")]
        [Filter(typeof(StubFilterA))]
        public static string Handle()
        {
            return "Hello, world!";
        }
    }

    internal class StubEndpointWithFilterNoImplementation
    {
        [Get("/")]
        [Filter(typeof(StubFilterNoImplementation))]
        public static string Handle()
        {
            return "Hello, world!";
        }
    }

    internal class StubEndpointWithMultipleFilters
    {
        [Get("/")]
        [Filter(typeof(StubFilterA), 100)]
        [Filter(typeof(StubFilterD), -1)]
        public static string Handle()
        {
            return "Hello, world!";
        }
    }

    internal class StubEndpointWithValidation
    {
        [Post("/")]
        public static string Handle(StubDto dto)
        {
            return dto.Message;
        }
    }

    internal class StubEndpointWithValidationNullableType
    {
        [Post("/")]
        public static string Handle(StubDto? dto)
        {
            return dto?.Message ?? "No message";
        }
    }

    internal class StubEndpointWithValidationMultipleTypes
    {
        [Post("/")]
        public static string Handle(StubDto dto, [AsParameters] StubQueryValues queryValues)
        {
            return $"{dto.Message}, {queryValues.Audience}!";
        }
    }

    internal class StubEndpointWithoutValidation
    {
        [Post("/")]
        public static string Handle(StubDtoNoValidator dto)
        {
            return dto.Message ?? "No message";
        }
    }

    internal class StubErrorResponseFactory : IErrorResponseFactory
    {
        public object? Create(ErrorContext<BadHttpRequestException> context)
        {
            throw new NotImplementedException();
        }

        public object? Create(ErrorContext<ValidationException> context)
        {
            return new { Errors = context.Exception.Errors.ToDictionary(e => e.PropertyName, e => e.ErrorMessage) };
        }

        public object? Create(ErrorContext<Exception> context)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubDtoValidator : AbstractValidator<StubDto>
    {
        public StubDtoValidator()
        {
            RuleFor(d => d.Message)
                .NotEmpty();
        }
    }

    internal class StubQueryValuesValidator : AbstractValidator<StubQueryValues>
    {
        public StubQueryValuesValidator()
        {
            RuleFor(q => q.Audience)
                .NotEmpty();
        }
    }

    internal record StubDto(string Message);

    internal record StubDtoNoValidator(string Message);

    internal record StubQueryValues([FromQuery(Name = "audience")] string Audience);
}
#endif
