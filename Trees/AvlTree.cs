using System;
using AmazonLogisticsTrees.Interfaces;
using AmazonLogisticsTrees.Models;

namespace AmazonLogisticsTrees.Trees
{
    public class AvlTree : IBalancedTree
    {
        private AvlNode? _root;
        public string TreeName => "Árvore AVL";
        public int RotationCount { get; private set; }

        public void Clear()
        {
            _root = null;
            RotationCount = 0;
        }

        public int GetHeight() => GetNodeHeight(_root);

        private int GetNodeHeight(AvlNode? node) => node?.Height ?? 0;

        private int GetBalanceFactor(AvlNode? node) =>
            node == null ? 0 : GetNodeHeight(node.Left) - GetNodeHeight(node.Right);

        private void UpdateHeight(AvlNode node) =>
            node.Height = 1 + Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right));

        // Rotacao Simples a Direita
        private AvlNode RotateRight(AvlNode y)
        {
            RotationCount++;
            AvlNode x = y.Left!;
            AvlNode? T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            UpdateHeight(y);
            UpdateHeight(x);

            return x;
        }

        // Rotacao Simples a Esquerda
        private AvlNode RotateLeft(AvlNode x)
        {
            RotationCount++;
            AvlNode y = x.Right!;
            AvlNode? T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            UpdateHeight(x);
            UpdateHeight(y);

            return y;
        }

        public bool Insert(int key)
        {
            if (Search(key)) return false;
            _root = InsertRec(_root, key);
            return true;
        }

        private AvlNode InsertRec(AvlNode? node, int key)
        {
            if (node == null) return new AvlNode(key);

            if (key < node.Key)
                node.Left = InsertRec(node.Left, key);
            else if (key > node.Key)
                node.Right = InsertRec(node.Right, key);

            UpdateHeight(node);
            return Balance(node);
        }

        public bool Remove(int key)
        {
            if (!Search(key)) return false;
            _root = RemoveRec(_root, key);
            return true;
        }

        private AvlNode? RemoveRec(AvlNode? node, int key)
        {
            if (node == null) return null;

            if (key < node.Key)
                node.Left = RemoveRec(node.Left, key);
            else if (key > node.Key)
                node.Right = RemoveRec(node.Right, key);
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    AvlNode? temp = node.Left ?? node.Right;
                    node = temp;
                }
                else
                {
                    AvlNode temp = GetMinValueNode(node.Right);
                    node.Key = temp.Key;
                    node.Right = RemoveRec(node.Right, temp.Key);
                }
            }

            if (node == null) return null;

            UpdateHeight(node);
            return Balance(node);
        }

        private AvlNode Balance(AvlNode node)
        {
            int balance = GetBalanceFactor(node);

            // Caso Esquerda-Esquerda (LL)
            if (balance > 1 && GetBalanceFactor(node.Left) >= 0)
                return RotateRight(node);

            // Caso Esquerda-Direita (LR)
            if (balance > 1 && GetBalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left!);
                return RotateRight(node);
            }

            // Caso Direita-Direita (RR)
            if (balance < -1 && GetBalanceFactor(node.Right) <= 0)
                return RotateLeft(node);

            // Caso Direita-Esquerda (RL)
            if (balance < -1 && GetBalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right!);
                return RotateLeft(node);
            }

            return node;
        }

        private AvlNode GetMinValueNode(AvlNode node)
        {
            AvlNode current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        public bool Search(int key) => SearchRec(_root, key);

        private bool SearchRec(AvlNode? node, int key)
        {
            if (node == null) return false;
            if (key == node.Key) return true;
            return key < node.Key ? SearchRec(node.Left, key) : SearchRec(node.Right, key);
        }

        public void PrintInOrder()
        {
            if (_root == null)
            {
                Console.WriteLine("[Arvore Vazia]");
                return;
            }
            PrintInOrderRec(_root);
            Console.WriteLine();
        }

        private void PrintInOrderRec(AvlNode? node)
        {
            if (node == null) return;
            PrintInOrderRec(node.Left);
            Console.Write($"{node.Key} ");
            PrintInOrderRec(node.Right);
        }

        public void PrintTree()
        {
            if (_root == null)
            {
                Console.WriteLine("[Arvore Vazia]");
                return;
            }
            PrintTreeRec(_root, "", true);
        }

        private void PrintTreeRec(AvlNode? node, string indent, bool last)
        {
            if (node != null)
            {
                Console.Write(indent);
                Console.Write(last ? "└── " : "├── ");
                Console.WriteLine($"{node.Key} (h={node.Height}, fb={GetBalanceFactor(node)})");

                indent += last ? "    " : "│   ";
                PrintTreeRec(node.Left, indent, false);
                PrintTreeRec(node.Right, indent, true);
            }
        }
    }
}