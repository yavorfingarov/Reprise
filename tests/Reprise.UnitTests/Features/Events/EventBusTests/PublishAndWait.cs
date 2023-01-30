namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    public class PublishAndWait : EventBusTestBase
    {

        [Fact]
        public async Task PayloadNull()
        {
            ConfigureServices();
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await ThrowsTask(() => messageBus.PublishAndWait(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public void NoHandlers()
        {
            ConfigureServices();
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            messageBus.PublishAndWait(new StubEvent());
        }

        [Fact]
        public async Task SyncHandler()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockSyncEventHandler), 500, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            Stopwatch.Start();
            await messageBus.PublishAndWait(new StubEvent());
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 400, 600);
            var handler = AbstractMockEventHandler.Instances.Single();

            await Verify(new { requestScopeIdentifier, handler });
        }

        [Fact]
        public async Task SyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockSyncEventHandler), 500, true));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await ThrowsTask(() => messageBus.PublishAndWait(new StubEvent()))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task AsyncHandler()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            Stopwatch.Start();
            await messageBus.PublishAndWait(new StubEvent());
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 400, 600);
            var handler = AbstractMockEventHandler.Instances.Single();

            await Verify(new { requestScopeIdentifier, handler });
        }

        [Fact]
        public async Task AsyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, true));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await ThrowsTask(() => messageBus.PublishAndWait(new StubEvent()))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task MultipleHandlers()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, false),
                new EventHandlerDescriptor(typeof(MockSyncEventHandler), 1_000, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_500, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            Stopwatch.Start();
            await messageBus.PublishAndWait(new StubEvent());
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 1_400, 1_600);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances });
        }

        [Fact]
        public async Task MultipleHandlersThrow()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, true),
                new EventHandlerDescriptor(typeof(MockSyncEventHandler), 1_500, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 2_000, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await ThrowsTask(() => messageBus.PublishAndWait(new StubEvent()))
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task CancelRequest()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_500, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            var cancellationTokenSource = new CancellationTokenSource(750);

            Stopwatch.Start();
            await messageBus.PublishAndWait(new StubEvent(), cancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 650, 850);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances });
        }
    }
}
