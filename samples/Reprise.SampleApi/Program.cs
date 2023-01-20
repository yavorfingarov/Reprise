using NLog.Web;
using SimpleRequestLogger;

namespace Reprise.SampleApi
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Logging.ClearProviders();

            builder.Host.UseNLog();

            builder.ConfigureServices();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseRequestLogging();

            app.UseExceptionHandling();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapEndpoints(options =>
            {
                options.RequireAuthorization();
                options.AddEndpointFilter<TraceIdFilter>();
                options.AddValidationFilter();
            });

            app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

            app.Run();
        }
    }
}
