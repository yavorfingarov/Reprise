using Extensions = Reprise.EndpointRouteBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class EndpointRouteBuilderExtensionsTests
    {
        private readonly Dictionary<(string Method, string Route), Type> _MappedRoutes = new()
        {
            [("GET", "/")] = typeof(EchoEndpoint)
        };

        [Fact]
        public Task MapEndpoints() => Verify(WebApplication.Create().MapEndpoints().DataSources);

        [Fact]
        public Task MapEndpoints_AppNull() => Throws(() => Extensions.MapEndpoints(null!));

        [Fact]
        public Task MapEndpointsAssembly() => Verify(CreateApp().MapEndpoints(typeof(Program).Assembly).DataSources);

        [Fact]
        public Task MapEndpointsAssembly_AppNull() => Throws(() => Extensions.MapEndpoints(null!, typeof(Program).Assembly));

        [Fact]
        public Task MapEndpointsAssembly_AssemblyNull() => Throws(() => WebApplication.Create().MapEndpoints(null!));

        [Fact]
        public Task GetHandlerInfo_AmbiguousMatch() => Throws(() => Extensions.GetHandlerInfo(typeof(BrokenEndpointA)));

        [Fact]
        public Task GetHandlerInfo_NoMatch() => Throws(() => Extensions.GetHandlerInfo(typeof(BrokenEndpointB)));

        [Fact]
        public Task GetRouteAndMethods_NoAttribute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod(nameof(BrokenEndpointC.NoAttribute))!, _MappedRoutes));

        [Fact]
        public Task GetRouteAndMethods_MultipleAttributes() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod(nameof(BrokenEndpointC.MultipleAttributes))!, _MappedRoutes));

        [Fact]
        public Task GetRouteAndMethods_EmptyHttpMethod() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod(nameof(BrokenEndpointC.EmptyHttpMethod))!, _MappedRoutes));

        [Fact]
        public Task GetRouteAndMethods_EmptyRoute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod(nameof(BrokenEndpointC.EmptyRoute))!, _MappedRoutes));

        [Fact]
        public Task GetRouteAndMethods_DuplicateRoute() => Throws(
            () => Extensions.GetRouteAndMethods(typeof(HelloEndpoint).GetMethod(nameof(HelloEndpoint.Handle))!, _MappedRoutes));

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
}
