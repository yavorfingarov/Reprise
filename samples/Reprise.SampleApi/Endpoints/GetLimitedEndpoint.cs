#if NET7_0
using Microsoft.AspNetCore.RateLimiting;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetLimitedEndpoint : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("FixedWindowOneSecond", fixedWindowOptions =>
                {
                    fixedWindowOptions.PermitLimit = 1;
                    fixedWindowOptions.Window = TimeSpan.FromSeconds(1);
                });
            });
        }

        [Get("/limited")]
        [AllowAnonymous]
        [EnableRateLimiting("FixedWindowOneSecond")]
        [Produces(StatusCodes.Status200OK)]
        [Tags("Public")]
        public static IResult Handle()
        {
            return Results.Ok();
        }
    }
}
#endif
