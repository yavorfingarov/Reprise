using Reprise.SampleApi.WeatherService;

namespace Reprise.SampleApi.Endpoints
{
    [Endpoint]
    public class GetWeather : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddWeatherService();
        }

        [Get("/weather")]
        public static async Task<WeatherForecast[]> Handle(IWeatherService weatherService)
        {
            return await weatherService.GetForecast();
        }
    }
}
