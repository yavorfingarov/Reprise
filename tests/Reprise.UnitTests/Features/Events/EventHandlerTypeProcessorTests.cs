namespace Reprise.UnitTests.Features.Events
{
    [UsesVerify]
    public class EventHandlerTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly EventHandlerTypeProcessor _Processor = new();

        public EventHandlerTypeProcessorTests()
        {
            _Builder.Services.Clear();
        }

        [Fact]
        public Task Process()
        {
            _Processor.Process(_Builder, typeof(StubEventHandler));

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Processor.Process(_Builder, GetType());

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    internal record StubEvent() : IEvent;

    internal class StubEventHandler : IEventHandler<StubEvent>
    {
        public Task Handle(StubEvent payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
