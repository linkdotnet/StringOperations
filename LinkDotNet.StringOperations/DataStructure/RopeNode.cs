namespace LinkDotNet.StringOperations.DataStructure
{
    internal class RopeNode
    {
        public RopeNode Left { get; set; }
        public RopeNode Right { get; set; }
        public string Fragment { get; set; }
        public int Weight { get; private set; }

        public void CalculateAndSetWeight()
        {
            Weight = Left == null ? Fragment.Length : GetWeightInternal(Left);
        }
        
        private static int GetWeightInternal(RopeNode node)
        {
            if (node.Left != null && node.Right != null)
            {
                return GetWeightInternal(node.Left) + GetWeightInternal(node.Right);
            }

            return node.Left != null ? GetWeightInternal(node.Left) : node.Fragment.Length;
        }
    } 
}