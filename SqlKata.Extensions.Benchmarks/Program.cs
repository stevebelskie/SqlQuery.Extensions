using BenchmarkDotNet.Running;

namespace SqlKata.Extensions.Benchmarks
{
    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<GenericSelectVsBase>();
        }
    }
}
