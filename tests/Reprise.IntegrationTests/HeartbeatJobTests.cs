using Reprise.SampleApi.Jobs;

namespace Reprise.IntegrationTests
{
    public class HeartbeatJobTests : TestBase
    {
        [Fact]
        public void Invocations()
        {
            Assert.True(BeforeStartJob.Invocations > 0);
        }
    }
}
