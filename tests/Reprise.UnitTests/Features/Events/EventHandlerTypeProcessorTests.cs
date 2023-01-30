namespace Reprise.UnitTests.Features.Events
{
    [UsesVerify]
    public class EventHandlerTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly EventHandlerTypeProcessor _Processor = new();

        [Fact]
        public Task Process()
        {
            _Builder.Services.Clear();

            _Processor.Process(_Builder, typeof(MockAsyncEventHandler));

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Builder.Services.Clear();

            _Processor.Process(_Builder, GetType());

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task PostProcess()
        {
            _Builder.Services.Clear();

            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }
}
