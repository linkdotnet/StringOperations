using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using LinkDotNet.StringOperations.Search;

namespace LinkDotNet.Benchmarks;

public class SearchTests
{
    private const string Text = "The quick brown fox jumps over the lazy dog maybe also a cat a sheep and another dog";
    private const string Word = "dog";

    [Benchmark]
    public bool KnuthMorrisPrattContains() => KnuthMorrisPratt.HasPattern(Text, Word);

    [Benchmark]
    public bool BoyerMooreContains() => BoyerMoore.HasPattern(Text, Word);

    [Benchmark]
    public bool ZAlgorithmContains() => ZAlgorithm.HasPattern(Text, Word);

    [Benchmark]
    public IList<int> KnuthMorrisPrattPrattFindAll() => KnuthMorrisPratt.FindAll(Text, Word).ToList();

    [Benchmark]
    public IList<int> BoyerMooreFindAll() => BoyerMoore.FindAll(Text, Word).ToList();

    [Benchmark]
    public IList<int> ZAlgorithmFindAll() => ZAlgorithm.FindAll(Text, Word).ToList();


}