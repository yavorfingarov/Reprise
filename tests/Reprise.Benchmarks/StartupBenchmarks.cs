using BenchmarkDotNet.Engines;

namespace Reprise.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, invocationCount: 32)]
    public class StartupBenchmarks : BenchmarksBase
    {
        [Benchmark]
        public HttpClient Reprise()
        {
            return CreateClient<Api.Program>();
        }

        [Benchmark]
        public HttpClient Carter()
        {
            return CreateClient<Api.Carter.Program>();
        }

        [Benchmark]
        public HttpClient FastEndpoints()
        {
            return CreateClient<Api.FastEndpoints.Program>();
        }

        [Benchmark]
        public HttpClient MinimalApis()
        {
            return CreateClient<Api.MinimalApis.Program>();
        }
    }
}
