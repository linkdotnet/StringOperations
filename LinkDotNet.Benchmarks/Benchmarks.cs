using BenchmarkDotNet.Running;

namespace LinkDotNet.Benchmarks;

internal static class Benchmarks
{
    internal static void Main()
    {
        // BenchmarkRunner.Run<RopeConcatTests>();
        // BenchmarkRunner.Run<SearchTests>();
        BenchmarkRunner.Run<TrieVsHashSet>();
    }
}