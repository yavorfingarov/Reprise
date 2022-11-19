namespace Reprise.IntegrationTests
{
    public class WeatherTests : TestBase
    {
        [Fact]
        public async Task Get()
        {
            await Verify(await Client.GetAsync("/weather"))
                .IgnoreMembersWithType<HttpContent>();
        }
    }
}
