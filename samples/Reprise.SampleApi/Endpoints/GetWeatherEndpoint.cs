using Microsoft.AspNetCore.Cors;
using Reprise.SampleApi.WeatherService;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetWeatherEndpoint : IServiceConfigurator
    {
        private const string _CorsPolicy = "WeatherCorsPolicy";

        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddWeatherService();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(_CorsPolicy, builder =>
                {
                    builder.WithOrigins("https://contoso.com");
                });
            });
        }

        [Get("/weather")]
        [AllowAnonymous]
        [EnableCors(_CorsPolicy)]
        [Filter(typeof(GreetingFilter))]
        [Produces(StatusCodes.Status200OK, typeof(WeatherForecast[]))]
        [Tags("Public")]
        public static async Task<WeatherForecast[]> Handle(IWeatherService weatherService)
        {
            return await weatherService.GetForecast();
        }
    }
}
