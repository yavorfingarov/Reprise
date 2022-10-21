using Extensions = Reprise.WebApplicationBuilderExtensions;

namespace Reprise.UnitTests
{
    [UsesVerify]
    public class WebApplicationBuilderExtensionsTests
    {
        private readonly WebApplicationBuilder _Builder;

        public WebApplicationBuilderExtensionsTests()
        {
            _Builder = WebApplication.CreateBuilder();
            _Builder.Services.Clear();
        }

        [Fact]
        public Task ConfigureServices() => Verify(_Builder.ConfigureServices().Services);

        [Fact]
        public Task ConfigureServices_BuilderNull() => Throws(() => Extensions.ConfigureServices(null!));

        [Fact]
        public Task ConfigureServicesAssembly() => Verify(_Builder.ConfigureServices(typeof(Program).Assembly).Services);

        [Fact]
        public Task ConfigureServicesAssembly_BuilderNull() => Throws(() => Extensions.ConfigureServices(null!, typeof(Program).Assembly));

        [Fact]
        public Task ConfigureServicesAssembly_AssemblyNull() => Throws(() => _Builder.ConfigureServices(null!));

        [Fact]
        public Task CreateConfigurator() => Throws(() => Extensions.CreateConfigurator(typeof(ServiceC)));
    }
}
