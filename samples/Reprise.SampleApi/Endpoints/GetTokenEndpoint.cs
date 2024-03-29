﻿using Reprise.SampleApi.Security;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetTokenEndpoint
    {
        [Get("/token")]
        [AllowAnonymous]
#if NET7_0
        [Filter(typeof(GreetingFilter))]
#endif
        [Produces(StatusCodes.Status200OK, typeof(TokenResponse))]
        [Tags("Public")]
        public static TokenResponse Handle(IJwtGenerator generator)
        {
            var (type, token) = generator.Generate();

            return new TokenResponse(type, token);
        }
    }

    public record TokenResponse(string Type, string Token);
}
