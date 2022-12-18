using System.Globalization;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetGreetingsEndpoint
    {
        [Get("/greetings")]
        [AllowAnonymous]
        [Produces(StatusCodes.Status200OK, typeof(IEnumerable<string>))]
        [Tags("Public")]
        public static IEnumerable<string> Handle(GreetingConfiguration configuration)
        {
            return configuration.Names.Select(name => string.Format(CultureInfo.InvariantCulture, configuration.Message, name));
        }
    }

    [Configuration("Greeting")]
    public class GreetingConfiguration
    {
        public string Message { get; set; } = null!;

        public List<string> Names { get; set; } = null!;
    }
}
