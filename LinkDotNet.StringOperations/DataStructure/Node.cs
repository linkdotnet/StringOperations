using System.Collections.Generic;

namespace LinkDotNet.StringOperations.DataStructure
{
    internal class Node
    {
        public char Character { get; set; }

        public Node Parent { get; set; }

        public IDictionary<char, Node> Children { get; set; } = new Dictionary<char, Node>();

        public bool IsLeaf { get; set; }

        public Node(char character)
        {
            Character = character;
        }
    }
}