using Microsoft.Extensions.Hosting;

namespace Reprise.UnitTests.Features.Jobs.JobRunnerTests
{
    public class StartAsync : JobRunnerTestBase
    {
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
            await JobRunner.StartAsync(CancellationToken.None);
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 600, 800);

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
            var exception = await Assert.ThrowsAnyAsync<Exception>(() => JobRunner.StartAsync(CancellationToken.None));
            Stopwatch.Stop();

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 600, 1_200);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider, exception })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task BeforeStartJobs_StartCancelled()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), true, false, null),
                new JobState(typeof(MediumMockJob), true, false, null),
                new JobState(typeof(LongMockJob), true, false, null));
            var cancellationTokenSource = new CancellationTokenSource(300);

            await JobRunner.StartAsync(cancellationTokenSource.Token);

            await Task.Delay(200);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider });
        }

        [Fact]
        public async Task OnStartJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, true, null),
                new JobState(typeof(MediumMockJob), false, true, null),
                new JobState(typeof(LongMockJob), false, true, null));

            Stopwatch.Start();
            await JobRunner.StartAsync(CancellationToken.None);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(800);

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
            await JobRunner.StartAsync(CancellationToken.None);
            Stopwatch.Stop();

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(1_200);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task OnStartJobs_ApplicationStop()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, true, null),
                new JobState(typeof(MediumMockJob), false, true, null),
                new JobState(typeof(LongMockJob), false, true, null));
            var hostApplicationLifetime = App.Services.GetRequiredService<IHostApplicationLifetime>();

            await JobRunner.StartAsync(CancellationToken.None);

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);

            await Task.Delay(300);
            hostApplicationLifetime.StopApplication();
            await Task.Delay(200);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider });
        }

        [Fact]
        public async Task ScheduledJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("45 * * * *")),
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("* * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            var invocations = 0;
            MockDateTimeProvider.Setup(d => d.UtcNow)
                .Returns(() => ++invocations <= 3 ? startTime : startTime.AddMilliseconds(Stopwatch.ElapsedMilliseconds));

            Stopwatch.Start();
            await JobRunner.StartAsync(CancellationToken.None);

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue");
        }

        [Fact]
        public async Task ScheduledThrowingJobs()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(ShortMockThrowingJob), false, false, CrontabSchedule.Parse("* * * * *")),
                new JobState(typeof(ShortMockThrowingJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("* * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            var invocations = 0;
            MockDateTimeProvider.Setup(d => d.UtcNow)
                .Returns(() => ++invocations <= 4 ? startTime : startTime.AddMilliseconds(Stopwatch.ElapsedMilliseconds));

            Stopwatch.Start();
            await JobRunner.StartAsync(CancellationToken.None);

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);
            await Task.Delay(600);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue")
                .IgnoreStackTrace();
        }

        [Fact]
        public async Task ScheduledJobs_ApplicationStop()
        {
            ConfigureServices(
                new JobState(typeof(ShortMockJob), false, false, CrontabSchedule.Parse("* * * * *")),
                new JobState(typeof(MediumMockJob), false, false, CrontabSchedule.Parse("15 * * * *")),
                new JobState(typeof(LongMockJob), false, false, CrontabSchedule.Parse("* * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            var invocations = 0;
            MockDateTimeProvider.Setup(d => d.UtcNow)
                .Returns(() => ++invocations <= 3 ? startTime : startTime.AddMilliseconds(Stopwatch.ElapsedMilliseconds));
            var hostApplicationLifetime = App.Services.GetRequiredService<IHostApplicationLifetime>();

            Stopwatch.Start();
            await JobRunner.StartAsync(CancellationToken.None);

            Assert.True(Stopwatch.ElapsedMilliseconds < 100);

            await Task.Delay(500);
            hostApplicationLifetime.StopApplication();
            await Task.Delay(200);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue");
        }

        [Fact]
        public async Task MixedJobs()
        {
            ConfigureServices(new JobState(typeof(ShortMockJob), true, true, CrontabSchedule.Parse("* * * * *")));
            var startTime = DateTime.Today
                .AddMinutes(15)
                .AddMilliseconds(-200);
            var invocations = 0;
            MockDateTimeProvider.Setup(d => d.UtcNow)
                .Returns(() => ++invocations <= 1 ? startTime : startTime.AddMilliseconds(Stopwatch.ElapsedMilliseconds));

            Stopwatch.Start();
            await JobRunner.StartAsync(CancellationToken.None);

            Assert.InRange(Stopwatch.ElapsedMilliseconds, 200, 400);
            await Task.Delay(600);

            await Verify(new { Jobs, MockTaskRunner, MockDateTimeProvider })
                .IgnoreMember("ReturnValue");
        }
    }
}
