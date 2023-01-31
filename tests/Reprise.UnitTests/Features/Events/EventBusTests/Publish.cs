using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    public class Publish : EventBusTestBase
    {
        [Fact]
        public async Task PayloadNull()
        {
            ConfigureServices();
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await Throws(() => messageBus.Publish(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public void NoHandlers()
        {
            ConfigureServices();
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            messageBus.Publish(new StubEvent());
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
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

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

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers })
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
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

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

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers })
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
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(1_800);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers })
                .IgnoreStackTrace();
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
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(2_300);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task ApplicationStop()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 500, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_500, false));
            var app = Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var hostApplicationLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

            hostApplicationLifetime.StopApplication();
            await Task.Delay(300);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Handlers });
        }
    }
}
