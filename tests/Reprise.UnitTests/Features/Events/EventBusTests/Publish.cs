namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    public class Publish : EventBusTestBase
    {
        [Fact]
        public async Task PayloadNull()
        {
            ConfigureServices();

            await Throws(() => EventBus.Publish(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public void NoHandlers()
        {
            ConfigureServices();

            EventBus.Publish(Event);
        }

        [Fact]
        public async Task MultipleHandlers()
        {
            ConfigureServices(
                new WorkerDescriptor(typeof(MockEventHandler), 200, false),
                new WorkerDescriptor(typeof(MockEventHandler), 400, false),
                new WorkerDescriptor(typeof(MockEventHandler), 600, false));

            Stopwatch.Start();
            EventBus.Publish(Event);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(800);

            Assert.True(EventHandlers.All(h => h.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { RequestScopeIdentifier, EventHandlers, MockTaskRunner });
        }

        [Fact]
        public async Task MultipleHandlersThrow()
        {
            ConfigureServices(
                new WorkerDescriptor(typeof(MockEventHandler), 200, true),
                new WorkerDescriptor(typeof(MockEventHandler), 400, false),
                new WorkerDescriptor(typeof(MockEventHandler), 600, false),
                new WorkerDescriptor(typeof(MockEventHandler), 800, true));

            Stopwatch.Start();
            EventBus.Publish(Event);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(1_200);

            Assert.True(EventHandlers.All(h => h.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { RequestScopeIdentifier, EventHandlers, MockTaskRunner })
                .IgnoreStackTrace();
        }
    }
}
