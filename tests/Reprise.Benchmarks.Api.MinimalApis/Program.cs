using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Reprise.Benchmarks.Api.MinimalApis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton<IValidator<EchoRequest>, EchoRequestValidator>();

            builder.Services.Configure<GreetingConfiguration>(
                builder.Configuration.GetSection("Greeting"),
                config => config.ErrorOnUnknownConfiguration = true);

            var app = builder.Build();

            app.MapPost("/echo/{routeValue}", (
                [FromHeader(Name = "header-value")] string headerValue,
                string routeValue,
                string queryValue,
                EchoRequest requestBody,
                IValidator<EchoRequest> validator,
                IOptions<GreetingConfiguration> greetingOptions) =>
                {
                    validator.ValidateAndThrow(requestBody);

                    return new EchoResponse(
                        headerValue,
                        routeValue,
                        queryValue,
                        requestBody.BodyValue,
                        greetingOptions.Value.Message);
                });

            app.Run();
        }
    }

    public class GreetingConfiguration
    {
        public string Message { get; set; } = null!;
    }

    public class EchoRequestValidator : AbstractValidator<EchoRequest>
    {
        public EchoRequestValidator()
        {
            RuleFor(r => r.BodyValue)
                .NotEmpty();
        }
    }

    public record EchoRequest(string BodyValue);

    public record EchoResponse(
        string HeaderValue,
        string RouteValue,
        string QueryValue,
        string BodyValue,
        string Greeting);
}
