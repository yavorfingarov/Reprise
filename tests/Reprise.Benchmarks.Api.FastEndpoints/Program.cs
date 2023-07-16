using Reprise.Benchmarks.Api.FastEndpoints.Endpoints;

namespace Reprise.Benchmarks.Api.FastEndpoints
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();

            builder.Services.AddFastEndpoints(options =>
            {
                options.DisableAutoDiscovery = true;
                options.Assemblies = new[] { typeof(Program).Assembly };
            });

            builder.Services.Configure<GreetingConfiguration>(
                builder.Configuration.GetSection("Greeting"),
                config => config.ErrorOnUnknownConfiguration = true);

            var app = builder.Build();

            app.UseFastEndpoints();

            app.Run();
        }
    }
}
