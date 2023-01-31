using System.Diagnostics;

namespace Reprise.UnitTests.Features.Events.EventBusTests
{
    [UsesVerify]
    public abstract class EventBusTestBase : IDisposable
    {
        internal WebApplicationBuilder Builder { get; } = WebApplication.CreateBuilder();

        internal LoggerProvider LoggerProvider { get; } = LoggerRecording.Start();

        internal EventHandlerTypeProcessor Processor { get; } = new();

        internal Stopwatch Stopwatch { get; } = new();

        public void Dispose()
        {
            AbstractMockEventHandler.Handlers.Clear();
            AbstractMockEventHandler.InstanceId = 0;
            GC.SuppressFinalize(this);
        }

        internal void ConfigureServices(params EventHandlerDescriptor[] handlerDescriptors)
        {
            Builder.Services.AddScoped<ServiceScopeIdentifier>();
            foreach (var handlerDescriptor in handlerDescriptors)
            {
                Builder.Services.AddScoped(serviceProvider =>
                {
                    var serivceScopeIdentifier = serviceProvider.GetRequiredService<ServiceScopeIdentifier>();
                    var handler = Activator.CreateInstance(handlerDescriptor.Type, new object[] { serivceScopeIdentifier, handlerDescriptor });

                    return (IEventHandler<StubEvent>)handler!;
                });
            }
            Builder.Services.AddScoped<IEventBus, EventBus>();
            Builder.Services.AddSingleton(LoggerProvider.CreateLogger<EventBus>());
        }
    }
}
