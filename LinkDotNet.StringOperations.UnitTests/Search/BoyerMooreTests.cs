using System.Linq;
using LinkDotNet.StringOperations.Search;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests.Search;

public class BoyerMooreTests
{
    [Fact]
    public void ShouldFindAllOccurrences()
    {
        const string text = "That is my text with the word text 3 times. That is why text again";
        const string pattern = "Text";

        var occurrences = BoyerMoore.FindAll(text, pattern, true).ToList();

        Assert.Equal(3, occurrences.Count);
        Assert.Equal(11, occurrences[0]);
        Assert.Equal(30, occurrences[1]);
        Assert.Equal(56, occurrences[2]);
    }

    [Fact]
    public void DoNotGoOutOfBounds()
    {
        const string text = "The quick brown fox jumps over the lazy dog maybe also a cat a sheep and another dog";
        const string word = "dog";

        var occurrences = BoyerMoore.FindAll(text, word).ToList();

        Assert.Equal(2, occurrences.Count);
    }

    [Fact]
    public void ShouldAbortOnFirstOccurence()
    {
        const string text = "That is my text with the word text 3 times. That is why text again";
        const string pattern = "Text";

        var occurrences = BoyerMoore.FindAll(text, pattern, true, true).ToList();

        Assert.Single(occurrences);
        Assert.Equal(11, occurrences[0]);
    }

    [Theory]
    [InlineData(null, "null")]
    [InlineData("null", null)]
    [InlineData("", "null")]
    [InlineData("null", "")]
    public void ShouldReturnEmptyOccurrences_WhenGivenNullOrEmpty(string text, string pattern)
    {
        var occurrences = BoyerMoore.FindAll(text, pattern);

        Assert.Empty(occurrences);
    }

    [Fact]
    public void GivenNoHit_ThenEmptyArray()
    {
        var occurrences = BoyerMoore.FindAll("Word", "Text");

        Assert.Empty(occurrences);
    }

    [Fact]
    public void GivenPatternLongerThanText_EmptyArray()
    {
        var occurrences = BoyerMoore.FindAll("t", "longer").ToList();

        Assert.Empty(occurrences);
    }
}