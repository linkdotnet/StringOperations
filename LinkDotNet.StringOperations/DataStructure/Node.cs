using System.Collections.Generic;

namespace LinkDotNet.StringOperations.DataStructure
{
    internal class Node
    {
        public IDictionary<char, Node> Children { get; set; } = new Dictionary<char, Node>();

        public bool IsLeaf { get; set; }
    }
}