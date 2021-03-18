using System;
using System.Collections.Generic;

namespace LinkDotNet.StringOperations.DataStructure
{
    public class Trie
    {
        private readonly bool _ignoreCase;
        private readonly Node _root = new Node();

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

        private static Node CreateOrGetNode(char currentCharacter, IDictionary<char, Node> children)
        {
            Node node;
            if (children.ContainsKey(currentCharacter))
            {
                node = children[currentCharacter];
            }
            else
            {
                node = new Node();
                children.Add(currentCharacter, node);
            }

            return node;
        }

        private Node FindNode(ReadOnlySpan<char> word)
        {
            var children = _root.Children;
            Node currentNode = null;

            foreach (var character in word)
            {
                var currentCharacter = _ignoreCase ? char.ToUpperInvariant(character) : character;
                if (children.ContainsKey(currentCharacter))
                {
                    currentNode = children[currentCharacter];
                    children = currentNode.Children;
                }
                else
                {
                    return null;
                }
            }

            return currentNode;
        }
    }
}