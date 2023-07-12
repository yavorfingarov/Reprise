namespace Reprise.UnitTests.ExtensionMethods
{
    [UsesVerify]
    public class EndpointRouteBuilderExtensionsTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        [Fact]
        public Task MapEndpoints()
        {
            var mockEndpointMapper = new MockEndpointMapper();
            _Builder.Services.AddSingleton<EndpointMapper>(mockEndpointMapper);
            var app = _Builder.Build();

            app.MapEndpoints();

            return Verify(mockEndpointMapper)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task MapEndpoints_AppNull()
        {
            return Throws(() => EndpointRouteBuilderExtensions.MapEndpoints(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpoints_NoMapper()
        {
            var app = _Builder.Build();

            return Throws(() => app.MapEndpoints())
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpointsOptions()
        {
            var mockEndpointMapper = new MockEndpointMapper();
            _Builder.Services.AddSingleton<EndpointMapper>(mockEndpointMapper);
            var app = _Builder.Build();

            app.MapEndpoints(options => options.RequireAuthorization());

            return Verify(mockEndpointMapper)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task MapEndpointsOptions_AppNull()
        {
            return Throws(() => EndpointRouteBuilderExtensions.MapEndpoints(null!, _ => { }))
                .IgnoreStackTrace();
        }

        [Fact]
        public Task MapEndpointsOptions_OptionsNull()
        {
            var mockEndpointMapper = new MockEndpointMapper();
            _Builder.Services.AddSingleton<EndpointMapper>(mockEndpointMapper);

            _Builder.Build().MapEndpoints(null!);

            return Verify(mockEndpointMapper)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task MapEndpointsOptions_NoMapper()
        {
            var app = _Builder.Build();

            return Throws(() => app.MapEndpoints(_ => { }))
                .IgnoreStackTrace();
        }
    }

    internal class MockEndpointMapper : EndpointMapper
    {
        public EndpointOptions EndpointOptions { get; private set; } = null!;

        public List<string?> Processors { get; private set; } = null!;

        public override void MapEndpoints(IEndpointRouteBuilder app, EndpointOptions options, IRouteHandlerBuilderProcessor[] processors)
        {
            EndpointOptions = options;
            Processors = processors.Select(p => p.GetType().FullName).ToList();
        }
    }
}
