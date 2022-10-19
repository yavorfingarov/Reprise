namespace Reprise.SampleApi
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.ConfigureServices();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI();

            app.MapEndpoints();

            app.Run();
        }
    }
}
