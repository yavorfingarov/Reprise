namespace Reprise.UnitTests.Common.TaskRunnerTests
{
    [UsesVerify]
    public abstract class TaskRunnerTestBase : IDisposable
    {
        internal TaskRunner TaskRunner { get; } = new();

        public void Dispose()
        {
            TaskRunner.Dispose();
            GC.SuppressFinalize(this);
        }

        internal static void TimerCallbackMethod(object? state)
        {
            var timerCallbackMethodState = (state as TimerCallbackMethodState)!;
            timerCallbackMethodState.Invocations++;
        }
    }

    internal class TimerCallbackMethodState
    {
        public int Invocations { get; set; }
    }
}
