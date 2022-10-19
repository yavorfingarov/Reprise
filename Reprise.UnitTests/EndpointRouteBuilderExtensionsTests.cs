using Reprise.SampleApi.Data;
using Reprise.SampleApi.WeatherService;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class EndpointRouteBuilderExtensionsTests
    {
        [Fact]
        public Task MapEndpoints() => Verify(WebApplication.Create().MapEndpoints().DataSources);

        [Fact]
        public Task MapEndpoints_WithAssembly() => Verify(CreateApp().MapEndpoints(typeof(Program).Assembly).DataSources);

        [Fact]
        public Task GetHandlerInfo_AmbiguousMatch() => Throws(() => EndpointRouteBuilderExtensions.GetHandlerInfo(typeof(BrokenEndpointA)));

        [Fact]
        public Task GetHandlerInfo_NoMatch() => Throws(() => EndpointRouteBuilderExtensions.GetHandlerInfo(typeof(BrokenEndpointB)));

        [Fact]
        public Task GetRouteAndMethods_NoAttribute() => Throws(
            () => EndpointRouteBuilderExtensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod("NoAttribute")!));

        [Fact]
        public Task GetRouteAndMethods_EmptyHttpMethod() => Throws(
            () => EndpointRouteBuilderExtensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod("EmptyHttpMethod")!));

        [Fact]
        public Task GetRouteAndMethods_EmptyRoute() => Throws(
            () => EndpointRouteBuilderExtensions.GetRouteAndMethods(typeof(BrokenEndpointC).GetMethod("EmptyRoute")!));

        [Fact]
        public Task CheckRoutes_Duplicate() => Throws(CheckDuplicateRoutes);

        [Fact]
        public Task CreateHandler() => Verify(EndpointRouteBuilderExtensions.CreateHandler(typeof(EchoEndpoint).GetMethod("Handle")!));

        [Theory]
        [InlineData("", "/")]
        [InlineData("   ", "/")]
        [InlineData("{id}", "/")]
        [InlineData("/", "/")]
        [InlineData("/{id}", "/")]
        [InlineData("/api", "/")]
        [InlineData("/api/{id}", "/")]
        [InlineData("/users/{id}", "Users")]
        [InlineData("/users", "Users")]
        [InlineData("/api/users", "Users")]
        [InlineData("/api/users/{id}", "Users")]
        public void GetTag(string route, string expected) => Assert.Equal(expected, EndpointRouteBuilderExtensions.GetTag(route));

        private static WebApplication CreateApp()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddSingleton<DataContext>();
            builder.Services.AddWeatherService();

            return builder.Build();
        }

        private static void CheckDuplicateRoutes()
        {
            var mappedRoutes = new Dictionary<(string Method, string Route), Type>()
            {
                [("GET", "/")] = typeof(HelloEndpoint)
            };
            EndpointRouteBuilderExtensions.CheckRoutes(mappedRoutes, "/", new[] { "POST", "GET" }, typeof(EchoEndpoint));
        }
    }
}
