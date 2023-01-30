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
            await Task.Delay(100);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(500);

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

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(100);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(500);

            await Verify(new { requestScopeIdentifier, handler })
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
            await Task.Delay(100);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(500);

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

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(100);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(500);

            await Verify(new { requestScopeIdentifier, handler })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task MultipleHandlers()
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

            Stopwatch.Start();
            messageBus.Publish(new StubEvent());
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(100);
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.Equal(HandlerStatus.Running, h.HandlerStatus));
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.False(h.IsDisposed));
            await Task.Delay(2_000);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances })
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
            await Task.Delay(100);
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.Equal(HandlerStatus.Running, h.HandlerStatus));
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.False(h.IsDisposed));
            await Task.Delay(500);
            hostApplicationLifetime.StopApplication();
            await Task.Delay(100);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances })
                .IgnoreStackTrace();
        }
    }
}
