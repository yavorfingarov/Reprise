using BenchmarkDotNet.Running;

namespace Reprise.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<StartupBenchmarks>(args: args);
            BenchmarkRunner.Run<RequestBenchmarks>(args: args);
        }
    }
}
