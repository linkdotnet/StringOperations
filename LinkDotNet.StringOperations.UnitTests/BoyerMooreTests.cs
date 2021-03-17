using System.Linq;
using LinkDotNet.StringOperations.Search;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
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
    }
}