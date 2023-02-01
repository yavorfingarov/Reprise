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
}
