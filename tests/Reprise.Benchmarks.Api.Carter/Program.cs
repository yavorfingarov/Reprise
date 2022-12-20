using Reprise.Benchmarks.Api.Carter.Modules;

namespace Reprise.Benchmarks.Api.Carter
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();

            builder.Services.AddCarter(new DependencyContextAssemblyCatalog(new[] { typeof(Program).Assembly }));

            builder.Services.Configure<GreetingConfiguration>(
                builder.Configuration.GetSection("Greeting"),
                config => config.ErrorOnUnknownConfiguration = true);

            var app = builder.Build();

            app.MapCarter();

            app.Run();
        }
    }
}
