using Microsoft.AspNetCore.Mvc.Testing;
using Reprise.SampleApi;

namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public abstract class TestBase
    {
        public HttpClient Client { get; }

        public TestBase()
        {
            Client = CreateClient();
        }

        public static HttpClient CreateClient()
        {
            var factory = new WebApplicationFactory<Program>();

            return factory.CreateClient();
        }
    }
}
