namespace Reprise.UnitTests
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ServiceA>();
            builder.Services.AddScoped<ServiceB>();
        }
    }

    public class ServiceA
    {
    }

    public class ServiceB
    {
    }

    public class ServiceC
    {
        public string Message { get; }

        public ServiceC(string message)
        {
            Message = message;
        }
    }

    public struct StructService : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
        }
    }

    [Endpoint]
    public class HelloEndpoint
    {
        [Get("/")]
        public static string Handle() => "Hello, world!";
    }

    [Endpoint]
    public class EchoEndpoint
    {
        [Get("/echo/{input}")]
        public static string Handle(string input) => input;
    }

    [Endpoint]
    public abstract class AbstractEndpoint
    {
    }

    public class BrokenEndpointA
    {
        public static string Handle() => "Hello, world!";

        public static string Handle(string input) => input;
    }

    public class BrokenEndpointB
    {
        public static string DontHandle() => "Hello, world!";
    }

    public class BrokenEndpointC
    {
        public static string NoAttribute() => "Hello, world!";

        [Get("/")]
        [Post("/")]
        public static string MultipleAttributes() => "Hello, world!";

        [Map(new[] { "GET", "" }, "/")]
        public static string EmptyHttpMethod() => "Hello, world!";

        [Get("")]
        public static string EmptyRoute() => "Hello, world!";
    }
}
