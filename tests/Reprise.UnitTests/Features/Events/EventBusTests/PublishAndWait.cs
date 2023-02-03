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

            await messageBus.PublishAndWait(new StubEvent());

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers });
        }

        [Fact]
        public async Task SyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockSyncEventHandler), 500, true));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var exception = await Assert.ThrowsAnyAsync<Exception>(() => messageBus.PublishAndWait(new StubEvent()));

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers, exception })
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

            await messageBus.PublishAndWait(new StubEvent());

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers });
        }

        [Fact]
        public async Task AsyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, true));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var exception = await Assert.ThrowsAnyAsync<Exception>(() => messageBus.PublishAndWait(new StubEvent()));

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers, exception })
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

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 1_450, 2_500);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers });
        }

        [Fact]
        public async Task MultipleHandlersThrow()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, false),
                new EventHandlerDescriptor(typeof(MockSyncEventHandler), 1_500, false),
                new EventHandlerDescriptor(typeof(MockSyncEventHandler), 2_000, true));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<Exception>(() => messageBus.PublishAndWait(new StubEvent()));
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 1_950, 4_000);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers, exception })
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

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 700, 950);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers });
        }
    }
}
