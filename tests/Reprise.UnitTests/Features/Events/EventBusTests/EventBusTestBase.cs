using System.Diagnostics;

namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    [UsesVerify]
    public abstract class EventBusTestBase : IDisposable
    {
        internal static IEnumerable<WorkerBase> EventHandlers => WorkerBase.Instances.OrderBy(w => w.WorkerStatus);

        internal static StubEvent Event => new();

        internal IServiceScope Scope { get; private set; } = null!;

        internal ServiceScopeIdentifier RequestScopeIdentifier { get; private set; } = null!;

        internal Mock<TaskRunner> MockTaskRunner { get; } = new();

        internal IEventBus EventBus { get; private set; } = null!;

        internal Stopwatch Stopwatch { get; } = new();

        public void Dispose()
        {
            Scope.Dispose();
            WorkerBase.Instances.Clear();
            GC.SuppressFinalize(this);
        }

        internal void ConfigureServices(params WorkerDescriptor[] workerDescriptors)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddScoped<ServiceScopeIdentifier>();
            foreach (var workerDescriptor in workerDescriptors)
            {
                builder.Services.AddScoped(serviceProvider =>
                {
                    var serviceScopeIdentifier = serviceProvider.GetRequiredService<ServiceScopeIdentifier>();
                    var handler = Activator.CreateInstance(workerDescriptor.Type, new object[] { serviceScopeIdentifier, workerDescriptor });

                    return (IEventHandler<StubEvent>)handler!;
                });
            }
            MockTaskRunner.Setup(t => t.WhenAll(It.IsAny<IEnumerable<Func<Task>>>()))
                .CallBase();
            MockTaskRunner.Setup(t => t.CreateTimer(It.IsAny<TimerCallback>(), It.IsAny<object?>(), It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .CallBase();
            MockTaskRunner.Setup(t => t.StopTimers())
                .CallBase();
            builder.Services.AddSingleton(MockTaskRunner.Object);
            builder.Services.AddScoped<IEventBus, EventBus>();
            var loggerProvider = LoggerRecording.Start();
            builder.Services.AddSingleton(loggerProvider.CreateLogger<EventBus>());
            var app = builder.Build();
            Scope = app.Services.CreateScope();
            RequestScopeIdentifier = Scope.ServiceProvider.GetRequiredService<ServiceScopeIdentifier>();
            EventBus = Scope.ServiceProvider.GetRequiredService<IEventBus>();
        }
    }

    internal record StubEvent() : IEvent;

    internal class MockEventHandler : WorkerBase, IEventHandler<StubEvent>
    {
        public MockEventHandler(ServiceScopeIdentifier serviceScopeIdentifier, WorkerDescriptor workerDescriptor) :
            base(serviceScopeIdentifier, workerDescriptor)
        {
        }

        public async Task Handle(StubEvent payload, CancellationToken cancellationToken)
        {
            await RunAsync(cancellationToken);
        }

#pragma warning disable CA1822 // Mark members as static
        public void Handle()
#pragma warning restore CA1822 // Mark members as static
        {
        }
    }
}
