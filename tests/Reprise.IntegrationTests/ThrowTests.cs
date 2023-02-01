namespace Reprise.IntegrationTests
{
    public class ThrowTests : TestBase
    {
        [Fact]
        public async Task Get()
        {
            await Verify(await Client.GetAsync("/throw"))
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }
    }
}
