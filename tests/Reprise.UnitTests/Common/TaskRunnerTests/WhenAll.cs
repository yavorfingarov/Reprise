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
        public async Task MultipleMethods()
        {
            var tasks = Enumerable.Range(0, _ExpectedInvocations)
                .Select(_ => TestMethod());

            _Stopwatch.Start();
            await TaskRunner.WhenAll(tasks);
            _Stopwatch.Stop();

            Assert.True(_Stopwatch.ElapsedMilliseconds < _Delay * (_ExpectedInvocations - 1));
            Assert.Equal(_ExpectedInvocations, _Invocations);
        }

        [Fact]
        public async Task MultipleMethodsThrow()
        {
            var tasks = Enumerable.Range(0, _ExpectedInvocations)
                .Select(i => TestMethod(throws: i % 2 == 0));

            _Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<AggregateException>(() => TaskRunner.WhenAll(tasks));
            _Stopwatch.Stop();

            Assert.True(_Stopwatch.ElapsedMilliseconds < _Delay * (_ExpectedInvocations - 1));
            Assert.Equal(_ExpectedInvocations, _Invocations);
            await Verify(new { exception })
                .IgnoreStackTrace();
        }

        private async Task TestMethod(bool throws = false)
        {
            Interlocked.Increment(ref _Invocations);
            await Task.Delay(_Delay);
            if (throws)
            {
                throw new Exception("Test message");
            }
        }
    }
}
