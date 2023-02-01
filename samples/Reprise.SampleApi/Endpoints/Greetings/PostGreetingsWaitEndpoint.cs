namespace Reprise.SampleApi.Endpoints.Greetings
{
    [Endpoint]
    public class PostGreetingsWaitEndpoint
    {
        [Post("/greetings/wait")]
        [AllowAnonymous]
        [Produces(StatusCodes.Status202Accepted)]
        public static async Task<IResult> Handle(Greeting greeting, IEventBus eventBus, CancellationToken cancellationToken)
        {
            await eventBus.PublishAndWait(greeting, cancellationToken);

            return Results.Accepted();
        }
    }
}
