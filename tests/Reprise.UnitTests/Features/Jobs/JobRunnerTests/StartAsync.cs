namespace Reprise.UnitTests.Features.Jobs.JobRunnerTests
{
    public class StartAsync : JobRunnerTestBase
    {
        private readonly CancellationTokenSource _CancellationTokenSource = new();

        [Fact]
        public async Task NoJobs()
        {
            ConfigureServices();

            await JobRunner.StartAsync(CancellationToken.None);
        }

        [Fact]
        public async Task BeforeStartJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), true, false, null),
                new JobState(typeof(MediumMockJob), true, false, null),
                new JobState(typeof(LongMockJob), true, false, null));

            Stopwatch.Start();
            await JobRunner.StartAsync(_CancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 550, 800);
            Assert.True(Jobs.All(j => j.CancellationToken == _CancellationTokenSource.Token));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider });
        }

        [Fact]
        public async Task BeforeStartThrowingJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), true, false, null),
                new JobState(typeof(MediumMockThrowingJob), true, false, null),
                new JobState(typeof(LongMockThrowingJob), true, false, null),
                new JobState(typeof(LongMockJob), true, false, null));

            Stopwatch.Start();
            var exception = await Assert.ThrowsAnyAsync<Exception>(() => JobRunner.StartAsync(_CancellationTokenSource.Token));
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 550, 1_200);
            Assert.True(Jobs.All(j => j.CancellationToken == _CancellationTokenSource.Token));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider, exception })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task OnStartJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, true, null),
                new JobState(typeof(MediumMockJob), false, true, null),
                new JobState(typeof(LongMockJob), false, true, null));

            Stopwatch.Start();
            await JobRunner.StartAsync(_CancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(800);

            Assert.True(Jobs.All(j => j.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider });
        }

        [Fact]
        public async Task OnStartThrowingJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, true, null),
                new JobState(typeof(MediumMockThrowingJob), false, true, null),
                new JobState(typeof(LongMockThrowingJob), false, true, null),
                new JobState(typeof(LongMockJob), false, true, null));

            Stopwatch.Start();
            await JobRunner.StartAsync(_CancellationTokenSource.Token);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(1_200);

            Assert.True(Jobs.All(j => j.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task ScheduledJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("45 * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            MockDateTimeProvider.SetupSequence(d => d.UtcNow)
                .Returns(startTime)
                .Returns(startTime)
                .Returns(startTime.AddMilliseconds(500));

            Stopwatch.Start();
            await JobRunner.StartAsync(_CancellationTokenSource.Token);

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(600);

            Assert.True(Jobs.All(j => j.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue");
        }

        [Fact]
        public async Task ScheduledThrowingJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(ShortMockThrowingJob), false, false, CrontabSchedule.Parse("* * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            MockDateTimeProvider.SetupSequence(d => d.UtcNow)
                .Returns(startTime)
                .Returns(startTime)
                .Returns(startTime.AddMilliseconds(500));

            Stopwatch.Start();
            await JobRunner.StartAsync(_CancellationTokenSource.Token);

            Assert.True(Stopwatch.ElapsedMilliseconds < 150);
            await Task.Delay(600);

            Assert.True(Jobs.All(j => j.CancellationToken == ApplicationLifetime.ApplicationStopping));
            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue")
                .IgnoreStackTrace();
        }
    }
}
