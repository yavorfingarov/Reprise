namespace Reprise.UnitTests.Features.Events
{
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

#pragma warning disable CA1822 // Mark members as static
        public void Handle()
#pragma warning restore CA1822 // Mark members as static
        {
        }

        public abstract Task Handle(StubEvent payload, CancellationToken cancellationToken);
    }

    internal class MockSyncEventHandler : AbstractMockEventHandler
    {
        public MockSyncEventHandler(ServiceScopeIdentifier serviceScopeIdentifier, EventHandlerDescriptor messageHandlerDescriptor) :
            base(serviceScopeIdentifier, messageHandlerDescriptor)
        {
        }

        public override Task Handle(StubEvent payload, CancellationToken cancellationToken)
        {
            HandlerStatus = HandlerStatus.Running;
            Thread.Sleep(Delay);
            if (Throws)
            {
                HandlerStatus = HandlerStatus.Faulted;
                throw new Exception($"Test message from {Name}");
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

        public override async Task Handle(StubEvent payload, CancellationToken cancellationToken)
        {
            HandlerStatus = HandlerStatus.Running;
            try
            {
                await Task.Delay(Delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                HandlerStatus = HandlerStatus.Cancelled;

                return;
            }
            if (Throws)
            {
                HandlerStatus = HandlerStatus.Faulted;
                throw new Exception($"Test message from {Name}");
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
        Cancelled,
        Done
    }

    internal record StubEvent() : IEvent;
}
