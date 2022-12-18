using Microsoft.AspNetCore.Mvc.Testing;

namespace Reprise.Benchmarks
{
    public abstract class BenchmarksBase
    {
        public static HttpClient CreateClient<T>() where T : class
        {
            var webApplicationFactory = new WebApplicationFactory<T>();

            return webApplicationFactory.CreateClient();
        }
    }
}
