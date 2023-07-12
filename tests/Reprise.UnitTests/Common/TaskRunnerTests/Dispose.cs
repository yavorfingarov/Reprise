namespace Reprise.UnitTests.Common.TaskRunnerTests
{
    public class Dispose : TaskRunnerTestBase
    {
        [Fact]
        public void NoTimers()
        {
            TaskRunner.Dispose();
        }

        [Fact]
        public void MultipleTimers()
        {
            var timers = Enumerable.Range(0, 5)
                .Select(_ => TaskRunner.CreateTimer(TimerCallbackMethod, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan))
                .ToList();

            TaskRunner.Dispose();

            foreach (var timer in timers)
            {
                Assert.Throws<ObjectDisposedException>(() => timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan));
            }
        }
    }
}
