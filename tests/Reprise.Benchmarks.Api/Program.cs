namespace Reprise.Benchmarks.Api
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();

            builder.ConfigureServices();

            var app = builder.Build();

            app.UseExceptionHandling();

            app.MapEndpoints(options => options.AddValidationFilter());

            app.Run();
        }
    }
}
