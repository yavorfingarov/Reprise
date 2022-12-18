using Microsoft.AspNetCore.Mvc;

namespace Reprise.Benchmarks.Api.Endpoints
{
    [Endpoint]
    public class EchoEndpoint
    {
        [Post("/echo/{routeValue}")]
        public static EchoResponse Handle(
            [FromHeader(Name = "header-value")] string headerValue,
            string routeValue,
            string queryValue,
            EchoRequest requestBody,
            GreetingConfiguration greetingConfiguration)
        {
            return new EchoResponse(
                headerValue,
                routeValue,
                queryValue,
                requestBody.BodyValue,
                greetingConfiguration.Message);
        }
    }

    [Configuration("Greeting")]
    public class GreetingConfiguration
    {
        public string Message { get; set; } = null!;
    }

    public record EchoRequest(string BodyValue);

    public class EchoRequestValidator : AbstractValidator<EchoRequest>
    {
        public EchoRequestValidator()
        {
            RuleFor(r => r.BodyValue)
                .NotEmpty();
        }
    }

    public record EchoResponse(
        string HeaderValue,
        string RouteValue,
        string QueryValue,
        string BodyValue,
        string Greeting);
}
