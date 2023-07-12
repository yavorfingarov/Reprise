using System.Collections.Concurrent;

namespace Reprise.UnitTests.TestHelpers
{
    internal record WorkerDescriptor(Type Type, int Delay, bool Throws);

    internal enum WorkerStatus
    {
        Initial,
        Faulted,
        Done
    }

    internal abstract class WorkerBase : IDisposable
    {
        public static ConcurrentBag<WorkerBase> Instances { get; } = new();

        public Guid ServiceScopeId { get; }

        public WorkerStatus WorkerStatus { get; private set; } = WorkerStatus.Initial;

        internal CancellationToken CancellationToken { get; private set; }

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

        public async Task Run(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
            await Task.Delay(_Delay, cancellationToken);
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
