namespace Reprise.IntegrationTests
{
    public class GreetingTests : TestBase
    {
        [Fact]
        public async Task Get() => await Verify(await Client.GetAsync("/greetings"));
    }
}
