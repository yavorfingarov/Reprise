#if NET7_0
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public sealed class Filters : IDisposable
    {
        private IHost? _Host;

        private HttpClient _Client = null!;

        [Fact]
        public async Task EndpointFilter()
        {
            await SetupHost(typeof(StubEndpointWithFilter));

            await Verify(await _Client.GetAsync("/"));
        }

        [Fact]
        public async Task EndpointFilter_NoImplementation()
        {
            await ThrowsTask(() => SetupHost(typeof(StubEndpointWithFilterNoImplementation)))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task GlobalEndpointFilters()
        {
            await SetupHost(typeof(StubEndpointWithoutFilter), options =>
            {
                options.AddEndpointFilter<StubFilterA>();
                options.AddEndpointFilter<StubFilterB>();
                options.AddEndpointFilter<StubFilterC>();
            });

            await Verify(await _Client.GetAsync("/"));
        }

        [Fact]
        public async Task GlobalEndpointFilters_EndpointFilter()
        {
            await SetupHost(typeof(StubEndpointWithFilter), options =>
            {
                options.AddEndpointFilter<StubFilterB>();
                options.AddEndpointFilter<StubFilterC>();
            });

            await Verify(await _Client.GetAsync("/"));
        }

        [Fact]
        public async Task ValidationFilter()
        {
            await SetupHost(typeof(StubEndpointWithValidation), options => options.AddValidationFilter());

            await Verify(await _Client.PostAsync("/", JsonContent.Create(new StubDto("Hello, world!"))));
        }

        [Fact]
        public async Task ValidationFilter_InvalidRequest()
        {
            await SetupHost(typeof(StubEndpointWithValidation), options => options.AddValidationFilter());

            await Verify(await _Client.PostAsync("/", JsonContent.Create(new StubDto(null!))));
        }

        [Fact]
        public async Task ValidationFilter_NullableType()
        {
            await SetupHost(typeof(StubEndpointWithValidationNullableType), options => options.AddValidationFilter());

            await Verify(await _Client.PostAsync("/", JsonContent.Create(new StubDto(null!))));
        }

        [Fact]
        public async Task ValidationFilter_NoValidator()
        {
            await SetupHost(typeof(StubEndpointWithoutValidation), options => options.AddValidationFilter());

            await Verify(await _Client.PostAsync("/", JsonContent.Create(new StubDtoNoValidator(null!))));
        }

        public void Dispose()
        {
            _Host?.Dispose();
        }

        private async Task SetupHost(Type endpointType, Action<EndpointOptions>? configureOptions = null)
        {
            var endpointMapper = new EndpointMapper();
            endpointMapper._EndpointTypes.Add(endpointType);
            _Host = await new HostBuilder()
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
            _Client = _Host.GetTestClient();
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

    internal record StubDto(string Message);

    internal record StubDtoNoValidator(string Message);
}
#endif
