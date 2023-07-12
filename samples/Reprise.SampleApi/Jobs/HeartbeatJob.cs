namespace Reprise.SampleApi.Jobs
{
    [RunBeforeStart]
    [RunOnStart]
    [Cron("* * * * *")]
    public class HeartbeatJob : IJob
    {
        public static int Invocations => _Invocations;

        private static int _Invocations;

        private readonly ILogger<HeartbeatJob> _Logger;

        public HeartbeatJob(ILogger<HeartbeatJob> logger)
        {
            _Logger = logger;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _Invocations);
            _Logger.LogInformation("HeartbeatJob invoked.");

            return Task.CompletedTask;
        }
    }
}
