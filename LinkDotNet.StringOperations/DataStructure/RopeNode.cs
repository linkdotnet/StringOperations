namespace LinkDotNet.StringOperations.DataStructure
{
    internal class RopeNode
    {
        public RopeNode Left { get; set; }
        public RopeNode Right { get; set; }
        public string Fragment { get; set; }
        public int Weight { get; set; }
    } 
}