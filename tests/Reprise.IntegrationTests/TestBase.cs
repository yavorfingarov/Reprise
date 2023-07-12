using Microsoft.AspNetCore.Mvc.Testing;
using Reprise.SampleApi;

namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public abstract class TestBase : IDisposable
    {
        public HttpClient Client { get; }

        private readonly WebApplicationFactory<Program> _WebApplicationFactory;

        public TestBase()
        {
            _WebApplicationFactory = new WebApplicationFactory<Program>();
            Client = _WebApplicationFactory.CreateClient();
        }

        public void Dispose()
        {
            _WebApplicationFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
