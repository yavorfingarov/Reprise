namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetGreetings
    {
        [Get("/greetings")]
        public static IEnumerable<string> Handle(GreetingConfiguration configuration)
        {
            return configuration.Names.Select(name => string.Format(configuration.Message, name));
        }
    }

    [Configuration("Greeting")]
    public class GreetingConfiguration
    {
        public string Message { get; set; } = null!;

        public List<string> Names { get; set; } = null!;
    }
}
