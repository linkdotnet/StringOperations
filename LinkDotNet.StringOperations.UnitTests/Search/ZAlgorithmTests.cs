using System.Linq;
using LinkDotNet.StringOperations.Search;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests.Search;

public class ZAlgorithmTests
{
    [Fact]
    public void ShouldFindAllOccurrences()
    {
        const string text = "That is my text with the word text 3 times. That is why text again";
        const string pattern = "Text";

        var occurrences = ZAlgorithm.FindAll(text, pattern, true).ToList();

        Assert.Equal(3, occurrences.Count);
        Assert.Equal(11, occurrences[0]);
        Assert.Equal(30, occurrences[1]);
        Assert.Equal(56, occurrences[2]);
    }

    [Fact]
    public void ShouldAbortOnFirstOccurence()
    {
        const string text = "That is my text with the word text 3 times. That is why text again";
        const string pattern = "Text";

        var occurrences = ZAlgorithm.FindAll(text, pattern, true, true).ToList();

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
        var occurrences = ZAlgorithm.FindAll(text, pattern);

        Assert.Empty(occurrences);
    }

    [Fact]
    public void ShouldReturnIfOccurrenceInText()
    {
        var occurrence = ZAlgorithm.HasPattern("KnuthMorrisPratt", "t");

        Assert.True(occurrence);
    }

    [Fact]
    public void GivenNoHit_ThenEmptyArray()
    {
        var occurrences = ZAlgorithm.FindAll("Word", "Text");

        Assert.Empty(occurrences);
    }

    [Fact]
    public void GivenPatternLongerThanText_EmptyArray()
    {
        var hasHit = ZAlgorithm.HasPattern("t", "longer");

        Assert.False(hasHit);
    }
}