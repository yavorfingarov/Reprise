using Microsoft.AspNetCore.Mvc;

namespace Reprise.Benchmarks.Api.Carter.Modules
{
    public class EchoModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/echo/{routeValue}", HandlePost);
        }

        private IResult HandlePost(
            [FromHeader(Name = "header-value")] string headerValue,
            string routeValue,
            string queryValue,
            EchoRequest requestBody,
            HttpRequest request,
            IOptions<GreetingConfiguration> greetingOptions)
        {
            var validationResult = request.Validate(requestBody);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest();
            }
            var response = new EchoResponse(
                headerValue,
                routeValue,
                queryValue,
                requestBody.BodyValue,
                greetingOptions.Value.Message);

            return Results.Ok(response);
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
