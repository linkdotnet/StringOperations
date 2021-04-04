using System;
using System.Text;

namespace LinkDotNet.StringOperations.DataStructure
{
    public class Rope
    {
        private string _fragment;
        private bool _hasToRecalculateWeights;
        private Rope _left;
        private Rope _right;
        private int _weight;

        private Rope() {}

        public char this[int index] => GetIndex(index);

        public Tuple<Rope, Rope> Split(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index can't be negative");
            }

            CheckRecalculation();
            
            return SplitRope(this, index);
            
            static Tuple<Rope, Rope> SplitRope(Rope node, int index)
            {
                if (node._left == null)
                {
                    if (index == node._weight - 1)
                    {
                        return new Tuple<Rope, Rope>(node, null); 
                    }

                    var item1 = Create(node._fragment.ToCharArray()[..(index+1)]);
                    var item2 = Create(node._fragment.ToCharArray()[(index+1)..node._weight]);
                    return new Tuple<Rope, Rope>(item1, item2);
                }

                if (index == node._weight - 1)
                {
                    return new Tuple<Rope, Rope>(node._left, node._right);
                }

                if (index < node._weight)
                {
                    var splitLeftSide = SplitRope(node._left, index);
                    return new Tuple<Rope, Rope>(splitLeftSide.Item1, splitLeftSide.Item2 + node._right);
                }

                var splitRightSide = SplitRope(node._right, index - node._weight);
                return new Tuple<Rope, Rope>(node._left + splitRightSide.Item1, splitRightSide.Item2);
            }
        }

        public Rope Insert(Rope other, int index)
        {
            var pair = Split(index);
            var left = pair.Item1 + other;
            return pair.Item2 != null ? left + pair.Item2 : left;
        }

        public Rope Delete(int startIndex, int length)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex), "Starting index can^t be negative");
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length has to be bigger 0");
            }
            
            CheckRecalculation();

            var beforeStartIndex = Split(startIndex - 1).Item1;
            var afterStartPlusLength = Split(startIndex + length - 1).Item2;

            return beforeStartIndex + afterStartPlusLength;
        }

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

        public static Rope Concat(Rope left, Rope right, bool recalculateWeights = false)
        {
            var rope = new Rope { _left = left, _right = right, _hasToRecalculateWeights = true };

            if (recalculateWeights)
            {
                rope.CalculateAndSetWeight();
            }

            return rope;
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

        private char GetIndex(int index)
        {
            if (_hasToRecalculateWeights)
            {
                CheckRecalculation();
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

        private void CalculateAndSetWeight()
        {
            _weight = _left == null ? _fragment.Length : GetWeightInternal(_left);
            
            static int GetWeightInternal(Rope node)
            {
                if (node._left != null && node._right != null)
                {
                    return GetWeightInternal(node._left) + GetWeightInternal(node._right);
                }

                return node._left != null ? GetWeightInternal(node._left) : node._fragment.Length;
            }
        }

        private void CheckRecalculation()
        {
            if (_hasToRecalculateWeights)
            {
                CalculateAndSetWeight();
                _hasToRecalculateWeights = false;
            }
        }
    } 
}