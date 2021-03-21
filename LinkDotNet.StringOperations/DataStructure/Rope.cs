using System;
using System.Text;

namespace LinkDotNet.StringOperations.DataStructure
{
    public class Rope
    {
        private Rope _left;
        private Rope _right;
        private string _fragment;
        private int _weight;
        private bool _hasToRecalculateWeights;

        private Rope() {}
        
        public char this[int index] => GetIndex(index);

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            AppendStrings(this, stringBuilder);

            return stringBuilder.ToString();
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
            var rope = new Rope { _left = left, _right = right, _hasToRecalculateWeights = true };

            return rope;
        }

        public void CalculateAndSetWeight()
        {
            _weight = _left == null ? _fragment.Length : GetWeightInternal(_left);
        }

        public static Rope Create(ReadOnlySpan<char> text, int leafLength = 8)
        {
            return CreateInternal(text, leafLength, 0 , text.Length - 1);
        }

        private static Rope CreateInternal(ReadOnlySpan<char> text, int leafLength, int leftIndex, int rightIndex)
        {
            var node = new Rope();

            if (rightIndex - leftIndex > leafLength)
            {
                var center = (rightIndex + leftIndex + 1) / 2;
                node._left = CreateInternal(text, leafLength, leftIndex, center);
                node._right = CreateInternal(text, leafLength, center + 1, rightIndex);
            }
            else
            {
                var rightIndexInclusiveUpperBound = rightIndex + 1;
                node._fragment = text[leftIndex .. rightIndexInclusiveUpperBound].ToString();
            }
            
            node.CalculateAndSetWeight();
            
            return node;
        }

        private static int GetWeightInternal(Rope node)
        {
            if (node._left != null && node._right != null)
            {
                return GetWeightInternal(node._left) + GetWeightInternal(node._right);
            }

            return node._left != null ? GetWeightInternal(node._left) : node._fragment.Length;
        }

        private char GetIndex(int index)
        {
            if (_hasToRecalculateWeights)
            {
                CalculateAndSetWeight();
                _hasToRecalculateWeights = false;
            }
            
            return GetIndexInternal(this, index);

            static char GetIndexInternal(Rope node, int index)
            {
                if (node._weight <= index && node._right != null)
                {
                    return GetIndexInternal(node._right, index - node._weight);
                }

                if (node._left != null)
                {
                    return GetIndexInternal(node._left, index);
                }

                return node._fragment[index];
            }
        }

        private static void AppendStrings(Rope node, StringBuilder builder)
        {
            if (node == null)
            {
                return;
            }

            if (node._left == null && node._right == null)
            {
                builder.Append(node._fragment);
            }

            AppendStrings(node._left, builder);
            AppendStrings(node._right, builder);
        }
    } 
}