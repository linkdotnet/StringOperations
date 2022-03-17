using System;
using System.Linq;
using LinkDotNet.StringOperations.EditDistance;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests;

public class EditDistancesTests
{
    [Theory]
    [InlineData("Hello", "Hallo", false, "Hllo")]
    [InlineData("HeLlO", "hallo", true, "HLlO")]
    [InlineData("Hello", "hel", true, "Hel")]
    [InlineData("abc", "cbe", false, "b")]
    [InlineData("", "", false, "")]
    [InlineData("Test", "", false, "")]
    public void CheckLongestCommonSubsequent(string one, string two, bool ignoreCase, string expected)
    {
        var actual = one.GetLongestCommonSubsequence(two, ignoreCase);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ReturnNullOnNullWhen_WhenCallingGetLongestCommonSubsequent()
    {
        Assert.Null("string".GetLongestCommonSubsequence(null));
    }

    [Fact]
    public void ShouldReturnNull_WhenNullValueForLongestCommonSubsequence()
    {
        Assert.Null("test".GetLongestCommonSubsequence(null));
        Assert.Null(((string)null).GetLongestCommonSubsequence("null"));
    }

    [Theory]
    [InlineData("Hallo", "Hello", false, 1)]
    [InlineData("hALLO", "Hello", true, 1)]
    [InlineData("", "Hello", false, 5)]
    [InlineData("Hallo", "", false, 5)]
    [InlineData("olleH", "Hello", false, 4)]
    [InlineData("ABCDEF", "abcdef", false, 6)]
    public void CheckLevenshteinDistance(string one, string two, bool ignoreCase, int expectedDistance)
    {
        var actual = one.GetLevenshteinDistance(two, ignoreCase);

        Assert.Equal(expectedDistance, actual);
    }

    [Fact]
    public void ShouldReturn_WhenAbortCostHasReached()
    {
        const int abortCost = 3;
        var cost = "ABCDEFGHIKLMN".GetLevenshteinDistance("abcdefghijlkm", abortCost: abortCost);

        Assert.Equal(cost, abortCost);
    }

    [Fact]
    public void ShouldThrow_WhenNullValueForLevenshtein()
    {
        Assert.Throws<ArgumentNullException>(() => "test".GetLevenshteinDistance(null));
        Assert.Throws<ArgumentNullException>(() => ((string) null).GetLevenshteinDistance("Test"));
    }

    [Theory]
    [InlineData("ThatIsAWord", "Word", false, "Word")]
    [InlineData("WordLonger", "LongerWord", false, "Longer")]
    public void CheckLongestSubstring(string one, string two, bool ignoreCase, string expectedSubstring)
    {
        var longestCommonSubstring = one.GetLongestCommonSubstring(two, ignoreCase);

        Assert.Equal(expectedSubstring, longestCommonSubstring);
    }

    [Fact]
    public void ShouldReturnNull_WhenNullValueForLongestCommonSubstring()
    {
        Assert.Null("test".GetLongestCommonSubstring(null));
        Assert.Null(((string)null).GetLongestCommonSubstring("null"));
    }

    [Theory]
    [InlineData("Hallo", "Hello", false, 1)]
    [InlineData("a", "abc", false, 0)]
    [InlineData("abc", "a", false, 2)]
    [InlineData("ABC", "abc", true, 0)]
    [InlineData("ABC", "abc", false, 3)]
    public void ShouldCalculateHammingDistance(string one, string two, bool ignoreCase, int expectedCost)
    {
        var actualCost = one.GetHammingDistance(two, ignoreCase);

        Assert.Equal(expectedCost, actualCost);
    }

    [Fact]
    public void ShouldThrow_WhenNullValueForHammingDistance()
    {
        Assert.Throws<ArgumentNullException>(() => "test".GetHammingDistance(null));
        Assert.Throws<ArgumentNullException>(() => ((string) null).GetHammingDistance("Test"));
    }

    [Fact]
    public void ShouldGetClosestWords()
    {
        var actual = "Hallo".GetClosestWords(2, false, "Hallo", "Auto", "Something else", "Haribo");

        Assert.NotNull(actual);
        var collection = actual.ToArray();
        Assert.NotEmpty(collection);
        Assert.Equal(2, collection.Length);
        Assert.Equal("Hallo", collection[0]);
        Assert.Equal("Haribo", collection[1]);
    }

    [Fact]
    public void ShouldReturnEmptyArrayWhenNoInput()
    {
        var actual = ((string) null).GetClosestWords(1, false, "H");

        Assert.Empty(actual);
    }

    [Fact]
    public void ShouldReturnEmptyArrayWhenWordsEmpty()
    {
        var actual = "Test".GetClosestWords(1, false);

        Assert.Empty(actual);
    }

    [Fact]
    public void ShouldCheckIfWordIsNull()
    {
        var actual = "Hallo".GetClosestWords(2, false, "Hallo", null).ToArray();

        Assert.Single(actual);
        Assert.Equal("Hallo", actual[0]);
    }

    [Fact]
    public void ShouldGetClosestWord()
    {
        var actual = "Hallo".GetClosestWord(false, "Hello", "Helbo");

        Assert.Equal("Hello", actual);
    }
}