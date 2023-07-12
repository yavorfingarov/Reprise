namespace Reprise
{
    internal class TaskRunner : IDisposable
    {
        private readonly List<Timer> _Timers = new();

        public virtual async Task WhenAll(IEnumerable<Func<Task>> tasks)
        {
            var whenAllTask = Task.WhenAll(tasks.Select(Task.Run));
            try
            {
                await whenAllTask;
            }
            catch (Exception)
            {
                throw whenAllTask.Exception!;
            }
        }

        public virtual Timer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        {
            var timer = new Timer(callback, state, dueTime, period);
            _Timers.Add(timer);

            return timer;
        }

        public virtual Task StopTimers()
        {
            foreach (var timer in _Timers)
            {
                timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            foreach (var timer in _Timers)
            {
                timer.Dispose();
            }
        }
    }
}
