using System.Text;
using BenchmarkDotNet.Attributes;
using LinkDotNet.StringOperations.DataStructure;

namespace LinkDotNet.Benchmarks;

public class RopeConcatTests
{
    [Benchmark(Baseline = true)]
    public void ConcatenateCLRStrings()
    {
        var clrString = "Test";
        for (var i = 0; i < 10000; i++)
        {
            clrString += $"some string{i}";
        }
    }

    [Benchmark]
    public void ConcatenateStringBuilder()
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < 10000; i++)
        {
            stringBuilder.Append($"some string{i}");
        }
    }

    [Benchmark]
    public void ConcatenateRope()
    {
        var rope = Rope.Create("Test");
        for (var i = 0; i < 10000; i++)
        {
            rope += $"some string{i}";
        }
    }
}