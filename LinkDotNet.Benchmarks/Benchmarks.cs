using BenchmarkDotNet.Running;

namespace LinkDotNet.Benchmarks;

internal static class Benchmarks
{
    internal static void Main()
    {
        BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run();
    }
}