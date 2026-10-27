namespace AmazonLogisticsTrees.Interfaces
{
    public interface IBalancedTree
    {
        string TreeName { get; }
        int RotationCount { get; }
        bool Insert(int key);
        bool Remove(int key);
        bool Search(int key);
        void PrintInOrder();
        void PrintTree();
        int GetHeight();
        void Clear();
    }
}