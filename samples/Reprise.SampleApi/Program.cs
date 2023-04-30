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
#if NET7_0
            app.UseRateLimiter();
#endif
            app.MapEndpoints(options =>
            {
                options.RequireAuthorization();
#if NET7_0
                options.AddEndpointFilter<TraceIdFilter>();
                options.AddValidationFilter();
#endif
            });

            app.Logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);

            app.Run();
        }
    }
}
