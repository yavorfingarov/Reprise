using System.Diagnostics;

namespace Reprise.UnitTests.Common.TaskRunnerTests
{
    public sealed class WhenAll : TaskRunnerTestBase
    {
        private const int _Delay = 500;

        private const int _ExpectedInvocations = 6;

        private readonly Stopwatch _Stopwatch = new();

        private int _Invocations;

        [Fact]
        public async Task AsyncMethods()
        {
            var tasks = Enumerable.Repeat(() => AsyncMethod(throws: false), _ExpectedInvocations);

            _Stopwatch.Start();
            await TaskRunner.WhenAll(tasks);
            _Stopwatch.Stop();

            AssertInvocations();
        }

        [Fact]
        public async Task SyncMethods()
        {
            var tasks = Enumerable.Repeat(() => SyncMethod(throws: false), _ExpectedInvocations);

            _Stopwatch.Start();
            await TaskRunner.WhenAll(tasks);
            _Stopwatch.Stop();

            AssertInvocations();
        }

        [Fact]
        public async Task MixedMethods()
        {
            var tasks = Enumerable.Repeat(() => AsyncMethod(throws: false), _ExpectedInvocations / 2)
                .Concat(Enumerable.Repeat(() => SyncMethod(throws: false), _ExpectedInvocations / 2));

            _Stopwatch.Start();
            await TaskRunner.WhenAll(tasks);
            _Stopwatch.Stop();

            AssertInvocations();
        }

        [Fact]
        public async Task AsyncMethodsThrow()
        {
            var tasks = Enumerable.Range(0, _ExpectedInvocations)
                .Select(i => (Func<Task>)(() => AsyncMethod(throws: i % 2 == 0)));

            _Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<AggregateException>(() => TaskRunner.WhenAll(tasks));
            _Stopwatch.Stop();

            AssertInvocations();
            await Verify(exception)
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task SyncMethodsThrow()
        {
            var tasks = Enumerable.Range(0, _ExpectedInvocations)
                .Select(i => (Func<Task>)(() => SyncMethod(throws: i % 2 == 0)));

            _Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<AggregateException>(() => TaskRunner.WhenAll(tasks));
            _Stopwatch.Stop();

            AssertInvocations();
            await Verify(exception)
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task MixedMethodsThrow()
        {
            var asyncTasks = Enumerable.Range(0, _ExpectedInvocations / 2)
                .Select(i => (Func<Task>)(() => AsyncMethod(throws: i % 2 == 0)));
            var syncTasks = Enumerable.Range(0, _ExpectedInvocations / 2)
                .Select(i => (Func<Task>)(() => SyncMethod(throws: i % 2 == 0)));
            var tasks = asyncTasks.Concat(syncTasks);

            _Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<AggregateException>(() => TaskRunner.WhenAll(tasks));
            _Stopwatch.Stop();

            AssertInvocations();
            await Verify(exception)
                .IgnoreStackTrace();
        }

        private void AssertInvocations()
        {
            Assert.True(_Stopwatch.ElapsedMilliseconds < _Delay * _ExpectedInvocations / 2);
            Assert.Equal(_ExpectedInvocations, _Invocations);
        }

        private async Task AsyncMethod(bool throws)
        {
            Interlocked.Increment(ref _Invocations);
            await Task.Delay(_Delay);
            if (throws)
            {
                throw new Exception("TestMessage");
            }
        }

        private Task SyncMethod(bool throws)
        {
            Interlocked.Increment(ref _Invocations);
            Thread.Sleep(_Delay);
            if (throws)
            {
                throw new Exception("TestMessage");
            }

            return Task.CompletedTask;
        }
    }
}
