using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using LinkDotNet.StringOperations.DataStructure;

namespace LinkDotNet.Benchmarks;

[MemoryDiagnoser]
public class TrieVsHashSet
{
    private readonly HashSet<string> _hashSet = new();
    private readonly Trie _trie = new();

    [GlobalSetup]
    public void Setup()
    {
        var wordsToAdd = File.ReadAllLines("1000words.txt");

        foreach (var word in wordsToAdd)
        {
            _hashSet.Add(word);
            _trie.Add(word);
        }
    }

    [Benchmark]
    public IList<string> FindAllInHashSet() => _hashSet.Where(h => h.StartsWith("Hel")).ToList();

    [Benchmark]
    public IList<string> FindAllInTrie() => _trie.GetWordsWithPrefix("Hel").ToList();

    [Benchmark]
    public bool FindOneInHashSet() => _hashSet.Any(h => h == "happy");

    [Benchmark]
    public bool FindOneInTrie() => _trie.Find("happy");
}