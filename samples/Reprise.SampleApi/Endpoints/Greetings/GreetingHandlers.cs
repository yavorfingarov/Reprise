namespace Reprise.SampleApi.Endpoints.Greetings
{
    public record Greeting(string Message) : IEvent;

    public abstract class AbstractGreetingHandler : IEventHandler<Greeting>
    {
        public abstract int Delay { get; }

        private readonly ILogger _Logger;

        public AbstractGreetingHandler(ILoggerFactory loggerFactory)
        {
            _Logger = loggerFactory.CreateLogger(GetType().FullName!);
        }

        public async Task Handle(Greeting payload, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(Delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                LogInformation("Task was cancelled.");

                return;
            }
            LogInformation($"Received greeting with message: {payload.Message}");
        }

        private void LogInformation(string message)
        {
            EventBus.LogInformation($"[{GetType().Name}] {message}");
            _Logger.LogInformation("[{HandlerType}] {Message}", GetType().Name, message);
        }
    }

    public class GreetingHandler : AbstractGreetingHandler
    {
        public override int Delay => 500;

        public GreetingHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }

    public class SlowerGreetingHandler : AbstractGreetingHandler
    {
        public override int Delay => 1_000;

        public SlowerGreetingHandler(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }
    }
}
