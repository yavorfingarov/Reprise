using Microsoft.Extensions.DependencyInjection;

namespace Reprise.SampleApi.WeatherService
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherService(this IServiceCollection services)
        {
            return services.AddScoped<IWeatherService, WeatherService>();
        }
    }

    public interface IWeatherService
    {
        Task<WeatherForecast[]> GetForecast();
    }

    public class WeatherService : IWeatherService
    {
        private static readonly string[] _Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecast()
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    _Summaries[Random.Shared.Next(_Summaries.Length)]
                ))
                .ToArray();

            return Task.FromResult(forecast);
        }
    }

    public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
