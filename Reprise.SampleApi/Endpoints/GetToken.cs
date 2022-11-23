using Reprise.SampleApi.Security;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetToken
    {
        [Get("/token")]
        [AllowAnonymous]
        [Filter(typeof(GreetingFilter))]
        public static TokenResponse Handle(IJwtGenerator generator)
        {
            var (type, token) = generator.Generate();

            return new TokenResponse(type, token);
        }
    }

    public record TokenResponse(string Type, string Token);
}
