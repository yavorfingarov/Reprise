namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    public class PublishAndWait : EventBusTestBase
    {
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

            EventBus.PublishAndWait(Event);
        }

        [Fact]
        public async Task MultipleHandlers()
        {
            ConfigureServices(
                new WorkerDescriptor(typeof(MockEventHandler), 200, false),
                new WorkerDescriptor(typeof(MockEventHandler), 400, false),
                new WorkerDescriptor(typeof(MockEventHandler), 600, false));

            Stopwatch.Start();
            await EventBus.PublishAndWait(Event);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 600, 800);

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
            var exception = await Assert.ThrowsAnyAsync<Exception>(() => EventBus.PublishAndWait(Event));
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 800, 1_200);

            await Verify(new { RequestScopeIdentifier, EventHandlers, MockTaskRunner, exception })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task CancelRequest()
        {
            ConfigureServices(
                new WorkerDescriptor(typeof(MockEventHandler), 200, false),
                new WorkerDescriptor(typeof(MockEventHandler), 400, true),
                new WorkerDescriptor(typeof(MockEventHandler), 600, false));
            var cancellationTokenSource = new CancellationTokenSource(300);

            Stopwatch.Start();
            await EventBus.PublishAndWait(Event, cancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 300, 500);

            await Verify(new { RequestScopeIdentifier, EventHandlers, MockTaskRunner });
        }
    }
}
