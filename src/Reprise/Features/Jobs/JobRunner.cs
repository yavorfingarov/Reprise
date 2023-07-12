namespace Reprise
{
    internal sealed class JobRunner : IHostedService
    {
        private readonly ILogger<JobRunner> _Logger;

        private readonly IServiceScopeFactory _ServiceScopeFactory;

        private readonly IHostApplicationLifetime _HostApplicationLifetime;

        private readonly JobStateRegistry _JobStateRegistry;

        private readonly DateTimeProvider _DateTimeProvider;

        private readonly TaskRunner _TaskRunner;

        public JobRunner(
            ILogger<JobRunner> logger,
            IServiceScopeFactory serviceScopeFactory,
            IHostApplicationLifetime hostApplicationLifetime,
            JobStateRegistry jobStateRegistry,
            DateTimeProvider dateTimeProvider,
            TaskRunner taskRunner)
        {
            _Logger = logger;
            _ServiceScopeFactory = serviceScopeFactory;
            _HostApplicationLifetime = hostApplicationLifetime;
            _JobStateRegistry = jobStateRegistry;
            _DateTimeProvider = dateTimeProvider;
            _TaskRunner = taskRunner;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var runBeforeStartJobs = _JobStateRegistry
                .Where(j => j.RunBeforeStart)
                .Select(j => RunJob(j.JobType, cancellationToken));
            await _TaskRunner.WhenAll(runBeforeStartJobs);

            var scheduledJobStates = _JobStateRegistry
                .Where(j => j.RunOnStart || j.Schedule != null);
            foreach (var jobState in scheduledJobStates)
            {
                var dueTime = jobState.RunOnStart ? TimeSpan.Zero : CalculateDueTime(jobState.Schedule!, _DateTimeProvider.UtcNow);
                jobState.Timer = _TaskRunner.CreateTimer(RunScheduledJob, jobState, dueTime, Timeout.InfiniteTimeSpan);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _TaskRunner.StopTimers();
        }

        private async Task RunJob(Type jobType, CancellationToken cancellationToken)
        {
            using var scope = _ServiceScopeFactory.CreateScope();
            var job = (IJob)scope.ServiceProvider.GetRequiredService(jobType);
            await job.Run(cancellationToken);
        }

        private async void RunScheduledJob(object? state)
        {
            using var scope = _ServiceScopeFactory.CreateScope();
            var jobState = (state as JobState)!;
            var job = (IJob)scope.ServiceProvider.GetRequiredService(jobState.JobType);
            if (jobState.Schedule != null)
            {
                var dueTime = CalculateDueTime(jobState.Schedule, _DateTimeProvider.UtcNow);
                jobState.Timer!.Change(dueTime, Timeout.InfiniteTimeSpan);
            }
            try
            {
                await job.Run(_HostApplicationLifetime.ApplicationStopping);
            }
            catch (Exception ex)
            {
                _Logger.LogScheduledJobException(ex, jobState.JobType.FullName);
            }
        }

        private static TimeSpan CalculateDueTime(CrontabSchedule schedule, DateTime utcNow)
        {
            return schedule.GetNextOccurrence(utcNow) - utcNow;
        }
    }
}
