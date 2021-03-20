using BenchmarkDotNet.Running;

namespace LinkDotNet.Benchmarks
{
    internal class RopeBenchmarks
    {
        internal static void Main()
        {
            BenchmarkRunner.Run(typeof(RopeConcatTests).Assembly);
        }
    }
}