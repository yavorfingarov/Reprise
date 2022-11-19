namespace Reprise.IntegrationTests
{
    public class OpenApiTests : TestBase
    {
        [Fact]
        public async Task SwaggerJson()
        {
            await Verify(await Client.GetAsync("/swagger/v1/swagger.json"));
        }
    }
}
