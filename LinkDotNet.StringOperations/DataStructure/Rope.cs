using System;
using System.Text;

namespace LinkDotNet.StringOperations.DataStructure
{
    public class Rope
    {
        private RopeNode _root;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            AppendStrings(_root, stringBuilder);

            return stringBuilder.ToString();
        }

        public char this[int index] => Index(index);

        public static Rope Create(ReadOnlySpan<char> text, int leafLength = 8)
        {
            var rootNode = CreateInternal(text, leafLength, 0 , text.Length - 1);

            return new Rope {_root = rootNode};
        }

        public static Rope operator +(Rope left, Rope right)
        {
            return Concat(left, right);
        }

        public static Rope operator +(Rope left, ReadOnlySpan<char> right)
        {
            var rightRope = Create(right);

            return left + rightRope;
        }

        public static Rope operator +(ReadOnlySpan<char> left, Rope right)
        {
            var leftRope = Create(left);

            return leftRope + right;
        }

        public static Rope Concat(Rope left, Rope right)
        {
            var rope = new Rope { _root = new RopeNode { Left = left._root, Right = right._root } };
            return rope;

            static int RecalculateWeight(RopeNode nodeToRecalculate)
            {
                if (nodeToRecalculate.Left != null)
                {
                    return RecalculateWeight(nodeToRecalculate.Left);
                }

                return 0;
            }
        }

        private static RopeNode CreateInternal(ReadOnlySpan<char> text, int leafLength, int leftIndex, int rightIndex)
        {
            var node = new RopeNode();

            if (rightIndex - leftIndex > leafLength)
            {
                var center = (rightIndex + leftIndex + 1) / 2;
                node.Left = CreateInternal(text, leafLength, leftIndex, center);
                node.Right = CreateInternal(text, leafLength, center + 1, rightIndex);
            }
            else
            {
                var rightIndexInclusiveUpperBound = rightIndex + 1;
                node.Fragment = text[leftIndex .. rightIndexInclusiveUpperBound].ToString();
            }
            
            node.SetWeight();
            
            return node;
        }

        private static void AppendStrings(RopeNode node, StringBuilder builder)
        {
            if (node == null)
            {
                return;
            }

            if (node.Left == null && node.Right == null)
            {
                builder.Append(node.Fragment);
            }

            AppendStrings(node.Left, builder);
            AppendStrings(node.Right, builder);
        }

        private char Index(int index)
        {
            return IndexInternal(_root, index);

            static char IndexInternal(RopeNode node, int index)
            {
                if (node.Weight <= index && node.Right != null)
                {
                    return IndexInternal(node.Right, index - node.Weight);
                }

                if (node.Left != null)
                {
                    return IndexInternal(node.Left, index);
                }

                return node.Fragment[index];
            }
        }
    }
}