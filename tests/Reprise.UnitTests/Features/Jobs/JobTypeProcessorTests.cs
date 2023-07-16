namespace Reprise.UnitTests.Features.Jobs
{
    [UsesVerify]
    public class JobTypeProcessorTests
    {
        private readonly WebApplicationBuilder _Builder = WebApplication.CreateBuilder();

        private readonly JobTypeProcessor _Processor = new();

        public JobTypeProcessorTests()
        {
            _Builder.Services.Clear();
        }

        [Theory]
        [InlineData(typeof(StubJobRunBeforeStart))]
        [InlineData(typeof(StubJobRunOnStart))]
        [InlineData(typeof(StubJobCron))]
        [InlineData(typeof(StubJobAllTriggers))]
        public Task Process(Type jobType)
        {
            _Processor.Process(_Builder, jobType);

            return Verify(new { _Processor.JobStateRegistry, _Builder })
                .UniqueForRuntimeAndVersion()
                .UseParameters(jobType);
        }

        [Fact]
        public Task Process_NoImplementation()
        {
            _Processor.Process(_Builder, GetType());

            return Verify(new { _Processor.JobStateRegistry, _Builder })
                .UniqueForRuntimeAndVersion();
        }

        [Theory]
        [InlineData(typeof(StubJobEmptyCron))]
        [InlineData(typeof(StubJobInvalidCron))]
        [InlineData(typeof(StubJobNoTriggers))]
        public Task Process_InvalidTriggers(Type jobType)
        {
            return Throws(() => _Processor.Process(_Builder, jobType))
                .IgnoreStackTrace()
                .UseParameters(jobType);
        }

        [Fact]
        public Task PostProcess()
        {
            _Processor.PostProcess(_Builder);

            return Verify(_Builder)
                .UniqueForRuntimeAndVersion();
        }
    }

    [RunBeforeStart]
    internal class StubJobRunBeforeStart : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [RunOnStart]
    internal class StubJobRunOnStart : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Cron("* * * * *")]
    internal class StubJobCron : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [RunBeforeStart]
    [RunOnStart]
    [Cron("* * * * *")]
    internal class StubJobAllTriggers : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Cron("")]
    internal class StubJobEmptyCron : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    [Cron("foobar")]
    internal class StubJobInvalidCron : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    internal class StubJobNoTriggers : IJob
    {
        public Task Run(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
