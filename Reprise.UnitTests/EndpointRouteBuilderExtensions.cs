using Extensions = Reprise.EndpointRouteBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class EndpointRouteBuilderExtensions
    {
        private readonly Dictionary<(string Method, string Route), Type> _MappedRoutes = new()
        {
            [("GET", "/")] = typeof(EchoEndpoint)
        };

        [Fact]
        public Task MapEndpoints() => Verify(WebApplication.Create().MapEndpoints().DataSources);

        [Fact]
        public Task MapEndpoints_AppNull() => Throws(() => Extensions.MapEndpoints(null!)).IgnoreStackTrace();

        [Fact]
        public Task MapEndpointsAssembly() => Verify(CreateApp().MapEndpoints(typeof(Program).Assembly).DataSources);

        [Fact]
        public Task MapEndpointsAssembly_AppNull() => Throws(
            () => Extensions.MapEndpoints(null!, typeof(Program).Assembly))
                .IgnoreStackTrace();

        [Fact]
        public Task MapEndpointsAssembly_AssemblyNull() => Throws(() => WebApplication.Create().MapEndpoints(null!)).IgnoreStackTrace();

        [Fact]
        public Task GetHandlerInfo() => Verify(Extensions.GetHandlerInfo(typeof(HelloEndpoint)));

        [Fact]
        public Task GetHandlerInfo_AmbiguousMatch() => Throws(
            () => Extensions.GetHandlerInfo(typeof(StubEndpointMultipleHandle)))
                .IgnoreStackTrace();

        [Fact]
        public Task GetHandlerInfo_NoMatch() => Throws(() => Extensions.GetHandlerInfo(typeof(StubEndpointNoHandle))).IgnoreStackTrace();

        [Fact]
        public Task GetRouteAndMethods() =>
            Verify(Extensions.GetRouteAndMethods(typeof(EchoEndpoint).GetMethod(nameof(EchoEndpoint.Handle))!, _MappedRoutes));

        [Fact]
        public Task GetRouteAndMethods_NoAttribute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(StubEndpointBroken).GetMethod(nameof(StubEndpointBroken.NoAttribute))!, _MappedRoutes))
                .IgnoreStackTrace();

        [Fact]
        public Task GetRouteAndMethods_MultipleAttributes() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(StubEndpointBroken).GetMethod(nameof(StubEndpointBroken.MultipleAttributes))!, _MappedRoutes))
                .IgnoreStackTrace();

        [Fact]
        public Task GetRouteAndMethods_EmptyHttpMethod() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(StubEndpointBroken).GetMethod(nameof(StubEndpointBroken.EmptyHttpMethod))!, _MappedRoutes))
                .IgnoreStackTrace();

        [Fact]
        public Task GetRouteAndMethods_EmptyRoute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(StubEndpointBroken).GetMethod(nameof(StubEndpointBroken.EmptyRoute))!, _MappedRoutes))
                .IgnoreStackTrace();

        [Fact]
        public Task GetRouteAndMethods_DuplicateRoute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(HelloEndpoint).GetMethod(nameof(HelloEndpoint.Handle))!, _MappedRoutes))
                .IgnoreStackTrace();

        [Fact]
        public Task CreateHandler() => Verify(Extensions.CreateHandler(typeof(EchoEndpoint).GetMethod(nameof(EchoEndpoint.Handle))!));

        [Theory]
        [InlineData("", "/")]
        [InlineData("   ", "/")]
        [InlineData("{id}", "/")]
        [InlineData("/", "/")]
        [InlineData("/{id}", "/")]
        [InlineData("/api", "/")]
        [InlineData("/api/{id}", "/")]
        [InlineData("/users/{id}", "Users")]
        [InlineData("/{id}/users", "Users")]
        [InlineData("/users", "Users")]
        [InlineData("/api/users", "Users")]
        [InlineData("/api/users/{id}", "Users")]
        public void GetTag(string route, string expected) => Assert.Equal(expected, Extensions.GetTag(route));

        private static WebApplication CreateApp()
        {
            var builder = WebApplication.CreateBuilder();
            builder.ConfigureServices(typeof(Program).Assembly);

            return builder.Build();
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
    public abstract class StubEndpointAbstract
    {
    }

    public class StubEndpointMultipleHandle
    {
        public static string Handle() => "Hello, world!";

        public static string Handle(string input) => input;
    }

    public class StubEndpointNoHandle
    {
        public static string DontHandle() => "Hello, world!";
    }

    public class StubEndpointBroken
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
