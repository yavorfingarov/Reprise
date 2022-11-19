namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public class ServiceConfigurators
    {
        private readonly WebApplicationBuilder _Builder;

        private readonly ServiceConfiguratorTypeProcessor _Processor = new();

        public ServiceConfigurators()
        {
            _Builder = WebApplication.CreateBuilder();
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubServiceConfigurator));

            return Verify(_Builder);
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Processor.Process(_Builder, typeof(StubService));

            return Verify(_Builder);
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder);
        }
    }

    internal class StubServiceConfigurator : IServiceConfigurator
    {
        public void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<StubService>();
        }
    }

    internal class StubService
    {
    }
}
