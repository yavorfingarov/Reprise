namespace Reprise.SampleApi.Endpoints.Greetings
{
    [Endpoint]
    public class PostGreetingsEndpoint
    {
        [Post("/greetings")]
        [AllowAnonymous]
        [Produces(StatusCodes.Status202Accepted)]
        public static IResult Handle(Greeting greeting, IEventBus eventBus)
        {
            eventBus.Publish(greeting);

            return Results.Accepted();
        }
    }

    public record Greeting(string Message) : IEvent;

    public class GreetingHandler : IEventHandler<Greeting>
    {
        private readonly ILogger<GreetingHandler> _Logger;

        public GreetingHandler(ILogger<GreetingHandler> logger)
        {
            _Logger = logger;
        }

        public async Task Handle(Greeting payload, CancellationToken stoppingToken)
        {
            await Task.Delay(1_000, stoppingToken);
            EventBus.Greetings.Add(payload);
            _Logger.LogInformation("Received greeting with message: {Message}", payload.Message);
        }
    }

    public class SlowerGreetingHandler : IEventHandler<Greeting>
    {
        private readonly ILogger<GreetingHandler> _Logger;

        public SlowerGreetingHandler(ILogger<GreetingHandler> logger)
        {
            _Logger = logger;
        }

        public async Task Handle(Greeting payload, CancellationToken stoppingToken)
        {
            await Task.Delay(2_000, stoppingToken);
            var modifiedPayload = payload with { Message = payload.Message.ToUpperInvariant() };
            EventBus.Greetings.Add(modifiedPayload);
            _Logger.LogInformation("Received greeting with message: {Message}", modifiedPayload.Message);
        }
    }

    public static class EventBus
    {
        public static List<Greeting> Greetings { get; set; } = new List<Greeting>();
    }
}
