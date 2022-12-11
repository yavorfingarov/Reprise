namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetThrowEndpoint
    {
        [Get("/throw")]
        [AllowAnonymous]
        [ExcludeFromDescription]
        public static IResult Handle()
        {
            throw new NotImplementedException();
        }
    }
}
