namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetThrow
    {
        [Get("/throw")]
        [AllowAnonymous]
        public static IResult Handle()
        {
            throw new NotImplementedException();
        }
    }
}
