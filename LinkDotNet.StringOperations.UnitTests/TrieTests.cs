using System.Linq;
using LinkDotNet.StringOperations.DataStructure;
using Xunit;

namespace LinkDotNet.StringOperations.UnitTests
{
    public class TrieTests
    {
        [Theory]
        [InlineData("csharp", "csharp", false, true)]
        [InlineData("cccc", "ccccc", false, false)]
        [InlineData("words", "word", false, false)]
        [InlineData("WOrd", "word", true, true)]
        [InlineData("Word", "", true, false)]
        [InlineData("Word", null, true, false)]
        public void ShouldFindEntries(string wordToAdd, string wordToSearch, bool ignoreCase, bool expectedHit)
        {
            var trie = new Trie(ignoreCase);
            trie.Add(wordToAdd);

            var actualHit = trie.Find(wordToSearch);
            
            Assert.Equal(expectedHit, actualHit);
        }

        [Fact]
        public void GivenMultipleWords_ShouldFindNotSubstring()
        {
            var trie = new Trie();
            trie.Add("abcde");
            trie.Add("abcdefg");
            trie.Add("efgh");

            var hasHit = trie.Find("efg");

            Assert.False(hasHit);
        }

        [Theory]
        [InlineData("text", "te", false, true)]
        [InlineData("Text", "tE", true, true)]
        [InlineData("Word", "", true, false)]
        [InlineData("Word", null, true, false)]
        [InlineData("word", "word", false, true)]
        [InlineData("word", "words", false, false)]
        [InlineData("word", "odr", false, false)]
        public void ShouldStartsWithEntries(string wordToAdd, string wordToSearch, bool ignoreCase, bool expectedHit)
        {
            var trie = new Trie(ignoreCase);
            trie.Add(wordToAdd);

            var actualHit = trie.StartsWith(wordToSearch);
            
            Assert.Equal(expectedHit, actualHit);
        }
        
        [Fact]
        public void GivenMultipleWords_ShouldNotFindStartsWithSubstring()
        {
            var trie = new Trie();
            trie.Add("abcde");
            trie.Add("abcdefg");
            trie.Add("efgh");

            var hasHit = trie.StartsWith("def");

            Assert.False(hasHit);
        }

        [Fact]
        public void ShouldReturnAllWordsWithStartingPrefix()
        {
            var trie = new Trie();
            trie.Add("Hello");
            trie.Add("Helsinki");

            var hits = trie.GetWordsWithPrefix("Hel").ToList();
            
            Assert.Equal(2, hits.Count);
        }
    }
}