using System.Collections.Concurrent;

namespace Reprise.UnitTests.TestHelpers
{
    internal record WorkerDescriptor(Type Type, int Delay, bool Throws);

    internal enum WorkerStatus
    {
        Initial,
        Faulted,
        Cancelled,
        Done
    }

    internal abstract class WorkerBase : IDisposable
    {
        public static ConcurrentBag<WorkerBase> Instances { get; } = new();

        public Guid ServiceScopeId { get; }

        public WorkerStatus WorkerStatus { get; protected set; } = WorkerStatus.Initial;

        public bool IsDisposed { get; private set; }

        private readonly int _Delay;

        private readonly bool _Throws;

        public WorkerBase(ServiceScopeIdentifier serviceScopeIdentifier, WorkerDescriptor workerDescriptor)
        {
            ServiceScopeId = serviceScopeIdentifier.ScopeId;
            _Delay = workerDescriptor.Delay;
            _Throws = workerDescriptor.Throws;
            Instances.Add(this);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(_Delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                WorkerStatus = WorkerStatus.Cancelled;

                return;
            }
            if (_Throws)
            {
                WorkerStatus = WorkerStatus.Faulted;

                throw new Exception("Test message");
            }
            WorkerStatus = WorkerStatus.Done;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }
    }
}
