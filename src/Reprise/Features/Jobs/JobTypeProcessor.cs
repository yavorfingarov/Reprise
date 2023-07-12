namespace Reprise
{
    internal sealed class JobTypeProcessor : AbstractTypeProcessor
    {
        internal JobStateRegistry JobStateRegistry { get; } = new();

        public override void Process(WebApplicationBuilder builder, Type type)
        {
            if (type.IsAssignableTo(typeof(IJob)))
            {
                var jobState = CreateJobState(type);
                JobStateRegistry.Add(jobState);
                builder.Services.AddScoped(type);
            }
        }

        public override void PostProcess(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(JobStateRegistry);
            builder.Services.AddSingleton<DateTimeProvider>();
            builder.Services.TryAddSingleton<TaskRunner>();
            builder.Services.AddHostedService<JobRunner>();
        }

        private static JobState CreateJobState(Type type)
        {
            var runBeforeStart = type.IsDefined(typeof(RunBeforeStartAttribute));
            var runOnStart = type.IsDefined(typeof(RunOnStartAttribute));
            var cronAttribute = type.GetCustomAttribute<CronAttribute>();
            CrontabSchedule? schedule = null;
            if (cronAttribute != null)
            {
                if (cronAttribute.Expression.IsEmpty())
                {
                    throw new InvalidOperationException($"{type} has an empty cron expression.");
                }
                try
                {
                    schedule = CrontabSchedule.Parse(cronAttribute.Expression);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"{type} has an invalid cron expression.", ex);
                }
            }
            if (!runBeforeStart && !runOnStart && schedule == null)
            {
                throw new InvalidOperationException($"{type} has no run triggers.");
            }

            return new JobState(type, runBeforeStart, runOnStart, schedule);
        }
    }
}
