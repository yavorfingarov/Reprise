namespace Reprise.UnitTests.Features.Endpoints
{
    [UsesVerify]
    public class EndpointTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly EndpointTypeProcessor _Processor = new();

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubEndpointWithAttribute));

            return Verify(_Processor.EndpointMapper.EndpointTypes);
        }

        [Fact]
        public void Process_NoAttribute()
        {
            _Processor.Process(_Builder, typeof(StubEndpointWithoutAttribute));

            Assert.Empty(_Processor.EndpointMapper!.EndpointTypes!);
        }

        [Fact]
        public void PostProcess()
        {
            _Processor.PostProcess(_Builder);

            var app = _Builder.Build();
            var endpointMapper = app.Services.GetRequiredService<EndpointMapper>();
            Assert.Same(endpointMapper, _Processor.EndpointMapper);
        }
    }

    [Endpoint]
    internal class StubEndpointWithAttribute
    {
    }

    internal class StubEndpointWithoutAttribute
    {
    }
}
