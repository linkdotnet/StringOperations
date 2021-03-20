using System.Collections.Generic;

namespace LinkDotNet.StringOperations.DataStructure
{
    internal class TrieNode
    {
        public IDictionary<char, TrieNode> Children { get; set; } = new Dictionary<char, TrieNode>();

        public bool IsLeaf { get; set; }
    }
}