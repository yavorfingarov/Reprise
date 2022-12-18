using BenchmarkDotNet.Running;

namespace Reprise.Benchmarks
{
    public class Program
    {
        public static void Main()
        {
            BenchmarkRunner.Run<StartupBenchmarks>();
            BenchmarkRunner.Run<RequestBenchmarks>();
        }
    }
}
