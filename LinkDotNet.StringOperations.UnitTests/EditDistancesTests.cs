using System;
using LinkDotNet.StringOperations.EditDistance;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
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
    }
}