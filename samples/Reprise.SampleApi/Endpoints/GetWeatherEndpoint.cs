using Reprise.SampleApi.WeatherService;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetWeatherEndpoint : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddWeatherService();
        }

        [Get("/weather")]
        [AllowAnonymous]
        [Filter(typeof(GreetingFilter))]
        [Produces(StatusCodes.Status200OK, typeof(WeatherForecast[]))]
        [Tags("Public")]
        public static async Task<WeatherForecast[]> Handle(IWeatherService weatherService)
        {
            return await weatherService.GetForecast();
        }
    }
}
