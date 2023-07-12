namespace Reprise.UnitTests.Features.Jobs.JobRunnerTests
{
    public class StopAsync : JobRunnerTestBase
    {
        [Fact]
        public async Task CallsTaskRunner()
        {
            ConfigureServices();

            await JobRunner.StopAsync(CancellationToken.None);

            await Verify(new { MockTaskRunner });
        }
    }
}
