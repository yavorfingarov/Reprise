namespace Reprise
{
    internal sealed class JobState
    {
        public Type JobType { get; }

        public bool RunBeforeStart { get; }

        public bool RunOnStart { get; }

        public CrontabSchedule? Schedule { get; }

        public Timer? Timer { get; set; }

        public JobState(Type jobType, bool runBeforeStart, bool runOnStart, CrontabSchedule? schedule)
        {
            JobType = jobType;
            RunBeforeStart = runBeforeStart;
            RunOnStart = runOnStart;
            Schedule = schedule;
        }
    }
}
