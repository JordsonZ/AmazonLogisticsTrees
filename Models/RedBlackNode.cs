namespace AmazonLogisticsTrees.Models
{
    public class RedBlackNode
    {
        public int Key { get; set; }
        public NodeColor Color { get; set; }
        public RedBlackNode Left { get; set; }
        public RedBlackNode Right { get; set; }
        public RedBlackNode Parent { get; set; }

        public RedBlackNode(int key, NodeColor color, RedBlackNode nil)
        {
            Key = key;
            Color = color;
            Left = nil;
            Right = nil;
            Parent = nil;
        }
    }
}