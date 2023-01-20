using System.Diagnostics;
using System.Net.Http.Json;
using Reprise.SampleApi.Endpoints.Greetings;

namespace Reprise.IntegrationTests
{
    public sealed class GreetingsTests : TestBase, IDisposable
    {
        [Fact]
        public async Task Get()
        {
            await Verify(await Client.GetAsync("/greetings"))
                .ScrubMember("trace-id");
        }

        [Fact]
        public async Task Post()
        {
            var sw = Stopwatch.StartNew();
            var response = await Client.PostAsJsonAsync("/greetings", new Greeting("Hello, world!"));
            sw.Stop();

            Assert.True(sw.ElapsedMilliseconds < 500);
            await Task.Delay(2_000 + 150);
            await Verify(new { response, EventBus.Greetings })
                .ScrubMember("trace-id");
        }

        public void Dispose()
        {
            EventBus.Greetings.Clear();
        }
    }
}
