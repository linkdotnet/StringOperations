using System;
using System.Linq;
using LinkDotNet.StringOperations.Search;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
    public class KnuthMorrisPrattTests
    {
        [Fact]
        public void ShouldFindAllOccurrences()
        {
            const string text = "That is my text with the word text 3 times. That is why text again";
            const string pattern = "Text";

            var occurences = text.AsSpan().FindPatterns(pattern, true).ToList();
            
            Assert.Equal(3, occurences.Count);
            Assert.Equal(11, occurences[0]);
            Assert.Equal(30, occurences[1]);
            Assert.Equal(56, occurences[2]);
        }
        
        [Fact]
        public void ShouldAbortOnFirstOccurence()
        {
            const string text = "That is my text with the word text 3 times. That is why text again";
            const string pattern = "Text";

            var occurences = text.AsSpan().FindPatterns(pattern, true, true).ToList();
            
            Assert.Single(occurences);
            Assert.Equal(11, occurences[0]);
        }

        [Theory]
        [InlineData(null, "null")]
        [InlineData("null", null)]
        [InlineData("", "null")]
        [InlineData("null", "")]
        public void ShouldReturnEmptyOccurences_WhenGivenNullOrEmpty(string text, string pattern)
        {
            var occurences = KnuthMorrisPratt.FindPatterns(text, pattern);
            
            Assert.Empty(occurences);
        }

        [Fact]
        public void ShouldReturnIfOccurrenceInText()
        {
            var occurrence = "KnuthMorrisPratt".AsSpan().HasPattern("t");
            
            Assert.True(occurrence);
        }
    }
}