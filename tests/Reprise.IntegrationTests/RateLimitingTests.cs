#if NET7_0
namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public class RateLimitingTests : TestBase
    {
        [Fact]
        public async Task Get()
        {
            var response = await Client.GetAsync("/limited");
            response.EnsureSuccessStatusCode();
            await Task.Delay(1_100);

            await Verify(await Client.GetAsync("/limited"))
                .ScrubMember("trace-id");
        }

        [Fact]
        public async Task GetLimited()
        {
            var response = await Client.GetAsync("/limited");
            response.EnsureSuccessStatusCode();

            await Verify(await Client.GetAsync("/limited"))
                .ScrubMember("trace-id");
        }
    }
}
#endif
