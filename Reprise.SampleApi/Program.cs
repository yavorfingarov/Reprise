namespace Reprise.SampleApi
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.ConfigureServices();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseExceptionHandling();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapEndpoints(options =>
            {
                options.RequireAuthorization();
                options.AddEndpointFilter<TraceIdFilter>();
                options.AddValidationFilter();
            });

            app.Run();
        }
    }
}
