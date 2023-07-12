using System.Diagnostics;
using System.Net.Http.Json;
using Reprise.SampleApi.Endpoints.Greetings;

namespace Reprise.IntegrationTests
{
    public sealed class GreetingsTests : TestBase, IDisposable
    {
        private readonly Stopwatch _Stopwatch = new();

        [Fact]
        public async Task Get()
        {
            await Verify(await Client.GetAsync("/greetings"))
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public async Task Post()
        {
            _Stopwatch.Start();
            var response = await Client.PostAsJsonAsync("/greetings", new Greeting("Hello, world!"));
            _Stopwatch.Stop();

            Assert.True(_Stopwatch.ElapsedMilliseconds < 500);
            await Task.Delay(1_500);

            await Verify(new { response, GreetingsEventBus.Log })
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public async Task PostWait()
        {
            _Stopwatch.Start();
            var response = await Client.PostAsJsonAsync("/greetings/wait", new Greeting("Hello, world!"));
            _Stopwatch.Stop();

            Assert.InRange(_Stopwatch.ElapsedMilliseconds, 950, 1_500);

            await Verify(new { response, GreetingsEventBus.Log })
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }

        [Fact]
        public async Task PostWaitCancel()
        {
            var cancellationTokenSource = new CancellationTokenSource(750);
            _Stopwatch.Start();
            HttpResponseMessage? response = null;
            try
            {
                response = await Client.PostAsJsonAsync("/greetings/wait", new Greeting("Hello, world!"), cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            _Stopwatch.Stop();

            Assert.InRange(_Stopwatch.ElapsedMilliseconds, 700, 950);

            await Verify(new { response, GreetingsEventBus.Log })
                .ScrubMember("trace-id")
                .UniqueForRuntimeAndVersion();
        }

        public new void Dispose()
        {
            base.Dispose();
            GreetingsEventBus.ClearLog();
        }
    }
}
