using System.Diagnostics;
using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features.Jobs.JobRunnerTests
{
    [UsesVerify]
    public abstract class JobRunnerTestBase : IDisposable
    {
        internal static IEnumerable<WorkerBase> Jobs => WorkerBase.Instances.OrderBy(w => w.WorkerStatus);

        internal JobRunner JobRunner { get; private set; } = null!;

        internal Mock<DateTimeProvider> MockDateTimeProvider { get; } = new();

        internal Mock<TaskRunner> MockTaskRunner { get; } = new() { CallBase = true };

        internal IHostApplicationLifetime ApplicationLifetime { get; private set; } = null!;

        internal Stopwatch Stopwatch { get; } = new();

        public void Dispose()
        {
            WorkerBase.Instances.Clear();
            MockTaskRunner.Object.Dispose();
            GC.SuppressFinalize(this);
        }

        internal void ConfigureServices(params JobState[] jobStates)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddScoped<ServiceScopeIdentifier>();
            var jobStateRegistry = new JobStateRegistry();
            foreach (var jobState in jobStates)
            {
                jobStateRegistry.Add(jobState);
                builder.Services.AddScoped(jobState.JobType);
            }
            var loggerProvider = LoggerRecording.Start();
            builder.Services.AddSingleton(MockTaskRunner.Object);
            builder.Services.AddSingleton(loggerProvider.CreateLogger<JobRunner>());
            builder.Services.AddSingleton(jobStateRegistry);
            builder.Services.AddSingleton(MockDateTimeProvider.Object);
            builder.Services.AddSingleton<JobRunner>();
            var app = builder.Build();
            JobRunner = app.Services.GetRequiredService<JobRunner>();
            ApplicationLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        }
    }

    internal abstract class AbstractMockJob : WorkerBase, IJob
    {
        public AbstractMockJob(ServiceScopeIdentifier serviceScopeIdentifier, WorkerDescriptor workerDescriptor) :
            base(serviceScopeIdentifier, workerDescriptor)
        {
        }
    }

    internal class ShortMockJob : AbstractMockJob
    {
        public ShortMockJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(ShortMockJob), 200, false))
        {
        }
    }

    internal class MediumMockJob : AbstractMockJob
    {
        public MediumMockJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(MediumMockJob), 400, false))
        {
        }
    }

    internal class LongMockJob : AbstractMockJob
    {
        public LongMockJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(LongMockJob), 600, false))
        {
        }
    }

    internal class ShortMockThrowingJob : AbstractMockJob
    {
        public ShortMockThrowingJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(ShortMockThrowingJob), 200, true))
        {
        }
    }

    internal class MediumMockThrowingJob : AbstractMockJob
    {
        public MediumMockThrowingJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(MediumMockThrowingJob), 400, true))
        {
        }
    }

    internal class LongMockThrowingJob : AbstractMockJob
    {
        public LongMockThrowingJob(ServiceScopeIdentifier serviceScopeIdentifier) :
            base(serviceScopeIdentifier, new WorkerDescriptor(typeof(LongMockThrowingJob), 600, true))
        {
        }
    }
}
