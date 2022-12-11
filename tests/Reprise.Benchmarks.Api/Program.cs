namespace Reprise.Benchmarks.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.ConfigureServices();

            var app = builder.Build();

            app.MapEndpoints();

            app.Run();
        }
    }
}
