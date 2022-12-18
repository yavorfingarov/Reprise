namespace Reprise.Benchmarks.Api.FastEndpoints.Endpoints
{
    public class EchoEndpoint : Endpoint<EchoRequest, EchoResponse>
    {
        public required IOptions<GreetingConfiguration> GreetingOptions { get; set; }

        public override void Configure()
        {
            Post("/echo/{RouteValue}");
            AllowAnonymous();
        }

        public override Task HandleAsync(EchoRequest req, CancellationToken ct)
        {
            Response = new(
                req.HeaderValue,
                req.RouteValue,
                req.QueryValue,
                req.BodyValue,
                GreetingOptions.Value.Message);

            return Task.CompletedTask;
        }
    }

    public class GreetingConfiguration
    {
        public string Message { get; set; } = null!;
    }

    public class EchoRequest
    {
        [FromHeader("header-value")]
        public string HeaderValue { get; set; } = null!;

        public string RouteValue { get; set; } = null!;

        [BindFrom("queryValue")]
        public string QueryValue { get; set; } = null!;

        public string BodyValue { get; set; } = null!;
    }

    public class EchoRequestValidator : Validator<EchoRequest>
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
