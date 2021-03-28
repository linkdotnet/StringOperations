using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkDotNet.StringOperations.DataStructure
{
    public class Trie
    {
        private readonly bool _ignoreCase;
        private readonly TrieNode _root = new TrieNode();

        public Trie() : this(false)
        {
        }

        public Trie(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
        }
        
        public void Add(ReadOnlySpan<char> word)
        {
            var current = _root.Children;
            for (var i = 0; i < word.Length; i++)
            {
                var currentCharacter = _ignoreCase ? char.ToUpperInvariant(word[i]) : word[i];
                
                var node = CreateOrGetNode(currentCharacter, current);
                current = node.Children;

                if (i == word.Length - 1)
                {
                    node.IsLeaf = true;
                }
            }
        }

        public bool Find(ReadOnlySpan<char> word)
        {
            if (word.IsEmpty)
            {
                return false;
            }
            
            var node = FindNode(word);

            return node != null && node.IsLeaf;
        }

        public bool StartsWith(ReadOnlySpan<char> word)
        {
            if (word.IsEmpty)
            {
                return false;
            }

            return FindNode(word) != null;
        }

        public IEnumerable<string> GetWordsWithPrefix(string prefix)
        {
            var node = FindNode(prefix);
            if (node == null)
            {
                yield break;
            }

            foreach (var word in Collect(node, prefix.ToList()))
            {
                yield return word;
            }

            static IEnumerable<string> Collect(TrieNode node, List<char> prefix)
            {
                if (node.Children.Count == 0)
                {
                    yield return new string(prefix.ToArray());
                }

                foreach (var child in node.Children)
                {
                    prefix.Add(child.Key);
                    foreach (var t in Collect(child.Value, prefix))
                    {
                        yield return t;
                    }
                    prefix.RemoveAt(prefix.Count - 1);
                }
            }
        }

        private static TrieNode CreateOrGetNode(char currentCharacter, IDictionary<char, TrieNode> children)
        {
            TrieNode trieNode;
            if (children.ContainsKey(currentCharacter))
            {
                trieNode = children[currentCharacter];
            }
            else
            {
                trieNode = new TrieNode();
                children.Add(currentCharacter, trieNode);
            }

            return trieNode;
        }

        private TrieNode FindNode(ReadOnlySpan<char> word)
        {
            var children = _root.Children;
            TrieNode currentTrieNode = null;

            foreach (var character in word)
            {
                var currentCharacter = _ignoreCase ? char.ToUpperInvariant(character) : character;
                if (children.ContainsKey(currentCharacter))
                {
                    currentTrieNode = children[currentCharacter];
                    children = currentTrieNode.Children;
                }
                else
                {
                    return null;
                }
            }

            return currentTrieNode;
        }
    }
}