namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    public class PublishAndWait : EventBusTestBase
    {
        private readonly CancellationTokenSource _CancellationTokenSource = new();

        [Fact]
        public async Task PayloadNull()
        {
            ConfigureServices();

            await ThrowsTask(() => EventBus.PublishAndWait(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public void NoHandlers()
        {
            ConfigureServices();

            EventBus.PublishAndWait(Event, _CancellationTokenSource.Token);

            Assert.True(EventHandlers.All(h => h.CancellationToken == _CancellationTokenSource.Token));
        }

        [Fact]
        public async Task MultipleHandlers()
        {
            ConfigureServices(
                new WorkerDescriptor(typeof(MockEventHandler), 200, false),
                new WorkerDescriptor(typeof(MockEventHandler), 400, false),
                new WorkerDescriptor(typeof(MockEventHandler), 600, false));

            Stopwatch.Start();
            await EventBus.PublishAndWait(Event, _CancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 600, 800);
            Assert.True(EventHandlers.All(h => h.CancellationToken == _CancellationTokenSource.Token));
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
            var exception = await Assert.ThrowsAnyAsync<Exception>(() => EventBus.PublishAndWait(Event, _CancellationTokenSource.Token));
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 800, 1_200);
            Assert.True(EventHandlers.All(h => h.CancellationToken == _CancellationTokenSource.Token));
            await Verify(new { RequestScopeIdentifier, EventHandlers, MockTaskRunner, exception })
                .IgnoreStackTrace();
        }
    }
}
