using System.Net.Http.Json;

namespace Reprise.Benchmarks
{
    [MemoryDiagnoser]
    public class RequestBenchmarks : BenchmarksBase
    {
        public static HttpRequestMessage Request => CreateRequest();

        public HttpClient RepriseClient { get; } = CreateClient<Api.Program>();

        public HttpClient CarterClient { get; } = CreateClient<Api.Carter.Program>();

        public HttpClient FastEndpointsClient { get; } = CreateClient<Api.FastEndpoints.Program>();

        public HttpClient MinimalApisClient { get; } = CreateClient<Api.MinimalApis.Program>();

        [Benchmark]
        public async Task<HttpResponseMessage> Reprise()
        {
            return await RepriseClient.SendAsync(Request);
        }

        [Benchmark]
        public async Task<HttpResponseMessage> Carter()
        {
            return await CarterClient.SendAsync(Request);
        }

        [Benchmark]
        public async Task<HttpResponseMessage> FastEndpoints()
        {
            return await FastEndpointsClient.SendAsync(Request);
        }

        [Benchmark]
        public async Task<HttpResponseMessage> MinimalApis()
        {
            return await MinimalApisClient.SendAsync(Request);
        }

        private static HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/echo/RouteValue?queryValue=QueryValue")
            {
                Content = JsonContent.Create(new { BodyValue = "BodyValue" })
            };
            request.Headers.Add("header-value", "HeaderValue");

            return request;
        }
    }
}
