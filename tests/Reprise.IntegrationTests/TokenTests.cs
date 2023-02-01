namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public class TokenTests : TestBase
    {
        [Fact]
        public async Task GetToken()
        {
            await Verify(await Client.GetAsync("/token"))
                .ScrubMember("token")
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }
    }

    public record TokenResponse(string Type, string Token);
}
