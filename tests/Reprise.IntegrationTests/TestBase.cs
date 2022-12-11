using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Testing;
using Reprise.SampleApi;

namespace Reprise.IntegrationTests
{
    [UsesVerify]
    public abstract class TestBase
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyHttp.Enable();
        }

        public HttpClient Client { get; }

        public TestBase()
        {
            var factory = new WebApplicationFactory<Program>();
            Client = factory.CreateClient();
        }
    }
}
