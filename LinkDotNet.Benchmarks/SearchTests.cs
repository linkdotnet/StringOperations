using System.Linq;
using BenchmarkDotNet.Attributes;
using LinkDotNet.StringOperations.Search;

namespace LinkDotNet.Benchmarks;

public class SearchTests
{
    private const string Text = "The quick brown fox jumps over the lazy dog maybe also a cat a sheep and another dog";
    private const string Word = "dog";

    [Benchmark]
    public void KnuthMorrisPrattContains() => KnuthMorrisPratt.HasPattern(Text, Word);

    [Benchmark]
    public void BoyerMooreContains() => BoyerMoore.FindAll(Text, Word).Any();

    [Benchmark]
    public void KnuthMorrisPrattPrattFindAll() => KnuthMorrisPratt.FindAll(Text, Word).ToList();

    [Benchmark]
    public void BoyerMooreFindAll() => BoyerMoore.FindAll(Text, Word).ToList();

}