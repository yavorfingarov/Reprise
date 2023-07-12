namespace Reprise.SampleApi.Jobs
{
    [RunBeforeStart]
    public class BeforeStartJob : IJob
    {
        public static int Invocations => _Invocations;

        private static int _Invocations;

        private readonly ILogger<BeforeStartJob> _Logger;

        public BeforeStartJob(ILogger<BeforeStartJob> logger)
        {
            _Logger = logger;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _Invocations);
            _Logger.LogInformation("BeforeStartJob invoked.");

            return Task.CompletedTask;
        }
    }
}
