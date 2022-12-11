using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Reprise.Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        public static HttpRequestMessage Request => CreateRequest();

        public HttpClient RepriseFirstRequestClient { get; } = CreateClient<Api.Program>();

        public HttpClient MinimalApisFirstRequestClient { get; } = CreateClient<Api.MinimalApis.Program>();

        [Params("Startup", "Request")]
        public string? Type { get; set; }

        [Benchmark]
        public async Task<object> Reprise()
        {
            return Type switch
            {
                "Startup" => CreateClient<Api.Program>(),
                "Request" => await RepriseFirstRequestClient.SendAsync(Request),
                _ => throw new InvalidOperationException(),
            };
        }

        [Benchmark]
        public async Task<object> MinimalApis()
        {
            return Type switch
            {
                "Startup" => CreateClient<Api.MinimalApis.Program>(),
                "Request" => await MinimalApisFirstRequestClient.SendAsync(Request),
                _ => throw new InvalidOperationException(),
            };
        }

        private static HttpClient CreateClient<T>() where T : class
        {
            return new WebApplicationFactory<T>().CreateClient();
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
