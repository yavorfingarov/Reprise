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
        public Task ConfigureServices_WithAssembly() => Verify(_Builder.ConfigureServices(typeof(Program).Assembly).Services);

        [Fact]
        public Task CreateConfigurator() => Throws(() => WebApplicationBuilderExtensions.CreateConfigurator(typeof(ServiceC)));
    }
}
