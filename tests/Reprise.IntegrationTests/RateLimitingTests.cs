#if NET7_0
namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public class RateLimitingTests : TestBase
    {
        private readonly HttpClient _Client = CreateClient();

        [Fact]
        public async Task Get()
        {
            var response = await _Client.GetAsync("/limited");
            response.EnsureSuccessStatusCode();
            await Task.Delay(1_100);

            await Verify(await _Client.GetAsync("/limited"))
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public async Task GetLimited()
        {
            var response = await _Client.GetAsync("/limited");
            response.EnsureSuccessStatusCode();

            await Verify(await _Client.GetAsync("/limited"))
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }
    }
}
#endif
