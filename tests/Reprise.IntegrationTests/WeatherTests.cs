namespace Reprise.IntegrationTests
{
    public class WeatherTests : TestBase
    {
        [Theory]
        [InlineData("https://example.com")]
        [InlineData("https://contoso.com")]
        public async Task Get(string origin)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/weather");
            request.Headers.Add("Origin", origin);

            await Verify(await Client.SendAsync(request))
                .IgnoreMembersWithType<HttpContent>()
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion()
                .UseParameters(origin);
        }
    }
}
