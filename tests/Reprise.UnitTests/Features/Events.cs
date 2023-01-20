using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features
{
    [UsesVerify]
    public sealed class Events : IDisposable
    {
        private readonly WebApplicationBuilder _Builder;

        private readonly EventHandlerTypeProcessor _Processor = new();

        private readonly Mock<ILoggerFactory> _MockLoggerFactory = new();

        public Events()
        {
            _Builder = WebApplication.CreateBuilder();
            _MockLoggerFactory.Setup(m => m.CreateLogger(It.IsAny<string>()))
                .Returns(LoggerRecording.Start());
        }

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

        [Fact]
        public async Task Publish_PayloadNull()
        {
            ConfigureServices();
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            await Throws(() => messageBus.Publish(null!))
                .IgnoreStackTrace();
        }

        [Fact]
        public void Publish_NoHandlers()
        {
            ConfigureServices();
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            messageBus.Publish(new StubEvent());
        }

        [Fact]
        public async Task Publish_SyncHandler()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockSyncEventHandler), 1_000, false));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.NotStarted, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(1_000 + 150);

            await Verify(new { requestScopeIdentifier, handler });
        }

        [Fact]
        public async Task Publish_SyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockSyncEventHandler), 1_000, true));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.NotStarted, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(1_000 + 150);

            await Verify(new { requestScopeIdentifier, handler })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task Publish_AsyncHandler()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, false));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            await Task.Delay(150);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(1_000);

            await Verify(new { requestScopeIdentifier, handler });
        }

        [Fact]
        public async Task Publish_AsyncHandlerThrows()
        {
            ConfigureServices(new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, true));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            await Task.Delay(150);
            var handler = AbstractMockEventHandler.Instances.Single();
            Assert.Equal(HandlerStatus.Running, handler.HandlerStatus);
            Assert.False(handler.IsDisposed);
            await Task.Delay(1_000);

            await Verify(new { requestScopeIdentifier, handler })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task Publish_MultipleHandlers()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 2_000, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 3_000, false));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            await Task.Delay(150);
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.Equal(HandlerStatus.Running, h.HandlerStatus));
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.False(h.IsDisposed));
            await Task.Delay(3_000);

            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task Publish_ApplicationStop()
        {
            ConfigureServices(
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 1_000, false),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 2_000, true),
                new EventHandlerDescriptor(typeof(MockAsyncEventHandler), 3_000, false));
            var app = _Builder.Build();
            using var scope = app.Services.CreateScope();
            var requestScopeIdentifier = scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            var hostApplicationLifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
            var messageBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var sw = Stopwatch.StartNew();
            messageBus.Publish(new StubEvent());
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 150);
            await Task.Delay(150);
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.Equal(HandlerStatus.Running, h.HandlerStatus));
            Assert.All(AbstractMockEventHandler.Instances, h => Assert.False(h.IsDisposed));
            await Task.Delay(1_500);
            hostApplicationLifetime.StopApplication();
            await Task.Delay(150);
            await Verify(new { requestScopeIdentifier, AbstractMockEventHandler.Instances })
                .IgnoreStackTrace();
        }

        public void Dispose()
        {
            AbstractMockEventHandler.Instances.Clear();
            AbstractMockEventHandler.InstanceId = 0;
        }

        private void ConfigureServices(params EventHandlerDescriptor[] handlerDescriptors)
        {
            _Builder.Services.AddScoped<ServiceScopeIdentifier>();
            foreach (var handlerDescriptor in handlerDescriptors)
            {
                _Builder.Services.AddScoped(serviceProvider =>
                {
                    var serivceScopeIdentifier = serviceProvider.GetRequiredService<ServiceScopeIdentifier>();
                    var handler = Activator.CreateInstance(handlerDescriptor.Type, new object[] { serivceScopeIdentifier, handlerDescriptor });

                    return (IEventHandler<StubEvent>)handler!;
                });
            }
            _Builder.Services.AddSingleton<IEventBus, EventBus>();
            _Builder.Services.AddSingleton(_MockLoggerFactory.Object);
        }
    }

    internal class ServiceScopeIdentifier
    {
        public Guid ScopeId { get; } = Guid.NewGuid();
    }

    internal abstract class AbstractMockEventHandler : IEventHandler<StubEvent>, IDisposable
    {
        public static List<AbstractMockEventHandler> Instances { get; } = new();

        public static int InstanceId;

        public Guid ScopeId { get; }

        public string Name { get; }

        public HandlerStatus HandlerStatus { get; protected set; } = HandlerStatus.NotStarted;

        public bool IsDisposed { get; private set; }

        protected int Delay { get; }

        protected bool Throws { get; }

        public AbstractMockEventHandler(ServiceScopeIdentifier serviceScopeIdentifier, EventHandlerDescriptor messageHandlerDescriptor)
        {
            ScopeId = serviceScopeIdentifier.ScopeId;
            Name = $"{GetType().Name}{++InstanceId}";
            Delay = messageHandlerDescriptor.Delay;
            Throws = messageHandlerDescriptor.Throws;
            Instances.Add(this);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        public abstract Task Handle(StubEvent payload, CancellationToken stoppingToken);
    }

    internal class MockSyncEventHandler : AbstractMockEventHandler
    {
        public MockSyncEventHandler(ServiceScopeIdentifier serviceScopeIdentifier, EventHandlerDescriptor messageHandlerDescriptor) :
            base(serviceScopeIdentifier, messageHandlerDescriptor)
        {
        }

        public override Task Handle(StubEvent payload, CancellationToken stoppingToken)
        {
            HandlerStatus = HandlerStatus.Running;
            Thread.Sleep(Delay);
            if (Throws)
            {
                HandlerStatus = HandlerStatus.Faulted;
                throw new Exception("Test message");
            }
            HandlerStatus = HandlerStatus.Done;

            return Task.CompletedTask;
        }
    }

    internal class MockAsyncEventHandler : AbstractMockEventHandler
    {
        public MockAsyncEventHandler(ServiceScopeIdentifier serviceScopeIdentifier, EventHandlerDescriptor messageHandlerDescriptor) :
            base(serviceScopeIdentifier, messageHandlerDescriptor)
        {
        }

        public override async Task Handle(StubEvent payload, CancellationToken stoppingToken)
        {
            HandlerStatus = HandlerStatus.Running;
            await Task.Delay(Delay, stoppingToken);
            if (Throws)
            {
                HandlerStatus = HandlerStatus.Faulted;
                throw new Exception("Test message");
            }
            HandlerStatus = HandlerStatus.Done;
        }
    }

    internal record EventHandlerDescriptor(Type Type, int Delay, bool Throws);

    internal enum HandlerStatus
    {
        NotStarted,
        Running,
        Faulted,
        Done
    }

    internal record StubEvent() : IEvent;
}
