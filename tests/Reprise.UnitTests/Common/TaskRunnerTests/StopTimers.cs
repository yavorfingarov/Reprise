namespace Reprise.UnitTests.Common.TaskRunnerTests
{
    public class StopTimers : TaskRunnerTestBase
    {
        [Fact]
        public async Task MultipleTimers()
        {
            var states = Enumerable.Range(0, 5)
                .Select(_ => new TimerCallbackMethodState())
                .ToList();
            foreach (var state in states)
            {
                TaskRunner.CreateTimer(TimerCallbackMethod, state, TimeSpan.Zero, TimeSpan.FromMilliseconds(100));
            }
            await Task.Delay(50);

            await TaskRunner.StopTimers();

            await Task.Delay(150);
            Assert.True(states.Sum(s => s.Invocations) <= 5);
        }
    }
}
