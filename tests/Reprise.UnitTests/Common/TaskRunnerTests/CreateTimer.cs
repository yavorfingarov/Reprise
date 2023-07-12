namespace Reprise.UnitTests.Common.TaskRunnerTests
{
    public class CreateTimer : TaskRunnerTestBase
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

            await Task.Delay(250);
            Assert.True(states.Sum(s => s.Invocations) >= 10);
        }
    }
}
