using System;
using AmazonLogisticsTrees.Interfaces;
using AmazonLogisticsTrees.Models;

namespace AmazonLogisticsTrees.Trees
{
    public class RedBlackTree : IBalancedTree
    {
        private RedBlackNode _root;
        private readonly RedBlackNode _nil;

        public string TreeName => "Árvore Rubro-Negra";
        public int RotationCount { get; private set; }
        public int RecolorCount { get; private set; }

        public RedBlackTree()
        {
            _nil = new RedBlackNode(0, NodeColor.Black, null!);
            _nil.Left = _nil;
            _nil.Right = _nil;
            _nil.Parent = _nil;
            _root = _nil;
        }

        public void Clear()
        {
            _root = _nil;
            RotationCount = 0;
            RecolorCount = 0;
        }

        public int GetHeight() => GetNodeHeight(_root);

        private int GetNodeHeight(RedBlackNode node)
        {
            if (node == _nil) return 0;
            return 1 + Math.Max(GetNodeHeight(node.Left), GetNodeHeight(node.Right));
        }

        private void RotateLeft(RedBlackNode x)
        {
            RotationCount++;
            RedBlackNode y = x.Right;
            x.Right = y.Left;
            if (y.Left != _nil)
                y.Left.Parent = x;

            y.Parent = x.Parent;
            if (x.Parent == _nil)
                _root = y;
            else if (x == x.Parent.Left)
                x.Parent.Left = y;
            else
                x.Parent.Right = y;

            y.Left = x;
            x.Parent = y;
        }

        private void RotateRight(RedBlackNode y)
        {
            RotationCount++;
            RedBlackNode x = y.Left;
            y.Left = x.Right;
            if (x.Right != _nil)
                x.Right.Parent = y;

            x.Parent = y.Parent;
            if (y.Parent == _nil)
                _root = x;
            else if (y == y.Parent.Right)
                y.Parent.Right = x;
            else
                y.Parent.Left = x;

            x.Right = y;
            y.Parent = x;
        }

        public bool Insert(int key)
        {
            if (Search(key)) return false;

            RedBlackNode z = new RedBlackNode(key, NodeColor.Red, _nil);
            RedBlackNode y = _nil;
            RedBlackNode x = _root;

            while (x != _nil)
            {
                y = x;
                x = z.Key < x.Key ? x.Left : x.Right;
            }

            z.Parent = y;
            if (y == _nil)
                _root = z;
            else if (z.Key < y.Key)
                y.Left = z;
            else
                y.Right = z;

            InsertFixup(z);
            return true;
        }

        private void InsertFixup(RedBlackNode z)
        {
            while (z.Parent.Color == NodeColor.Red)
            {
                if (z.Parent == z.Parent.Parent.Left)
                {
                    RedBlackNode y = z.Parent.Parent.Right;
                    if (y.Color == NodeColor.Red)
                    {
                        z.Parent.Color = NodeColor.Black;
                        y.Color = NodeColor.Black;
                        z.Parent.Parent.Color = NodeColor.Red;
                        RecolorCount += 3;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Right)
                        {
                            z = z.Parent;
                            RotateLeft(z);
                        }
                        z.Parent.Color = NodeColor.Black;
                        z.Parent.Parent.Color = NodeColor.Red;
                        RecolorCount += 2;
                        RotateRight(z.Parent.Parent);
                    }
                }
                else
                {
                    RedBlackNode y = z.Parent.Parent.Left;
                    if (y.Color == NodeColor.Red)
                    {
                        z.Parent.Color = NodeColor.Black;
                        y.Color = NodeColor.Black;
                        z.Parent.Parent.Color = NodeColor.Red;
                        RecolorCount += 3;
                        z = z.Parent.Parent;
                    }
                    else
                    {
                        if (z == z.Parent.Left)
                        {
                            z = z.Parent;
                            RotateRight(z);
                        }
                        z.Parent.Color = NodeColor.Black;
                        z.Parent.Parent.Color = NodeColor.Red;
                        RecolorCount += 2;
                        RotateLeft(z.Parent.Parent);
                    }
                }
            }
            if (_root.Color != NodeColor.Black)
            {
                _root.Color = NodeColor.Black;
                RecolorCount++;
            }
        }

        private void Transplant(RedBlackNode u, RedBlackNode v)
        {
            if (u.Parent == _nil)
                _root = v;
            else if (u == u.Parent.Left)
                u.Parent.Left = v;
            else
                u.Parent.Right = v;
            v.Parent = u.Parent;
        }

        public bool Remove(int key)
        {
            RedBlackNode z = SearchNode(_root, key);
            if (z == _nil) return false;

            RedBlackNode y = z;
            NodeColor yOriginalColor = y.Color;
            RedBlackNode x;

            if (z.Left == _nil)
            {
                x = z.Right;
                Transplant(z, z.Right);
            }
            else if (z.Right == _nil)
            {
                x = z.Left;
                Transplant(z, z.Left);
            }
            else
            {
                y = GetMinimum(z.Right);
                yOriginalColor = y.Color;
                x = y.Right;
                if (y.Parent == z)
                {
                    x.Parent = y;
                }
                else
                {
                    Transplant(y, y.Right);
                    y.Right = z.Right;
                    y.Right.Parent = y;
                }
                Transplant(z, y);
                y.Left = z.Left;
                y.Left.Parent = y;
                y.Color = z.Color;
            }

            if (yOriginalColor == NodeColor.Black)
                DeleteFixup(x);

            return true;
        }

        private void DeleteFixup(RedBlackNode x)
        {
            while (x != _root && x.Color == NodeColor.Black)
            {
                if (x == x.Parent.Left)
                {
                    RedBlackNode w = x.Parent.Right;
                    if (w.Color == NodeColor.Red)
                    {
                        w.Color = NodeColor.Black;
                        x.Parent.Color = NodeColor.Red;
                        RecolorCount += 2;
                        RotateLeft(x.Parent);
                        w = x.Parent.Right;
                    }
                    if (w.Left.Color == NodeColor.Black && w.Right.Color == NodeColor.Black)
                    {
                        w.Color = NodeColor.Red;
                        RecolorCount++;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Right.Color == NodeColor.Black)
                        {
                            w.Left.Color = NodeColor.Black;
                            w.Color = NodeColor.Red;
                            RecolorCount += 2;
                            RotateRight(w);
                            w = x.Parent.Right;
                        }
                        w.Color = x.Parent.Color;
                        x.Parent.Color = NodeColor.Black;
                        w.Right.Color = NodeColor.Black;
                        RecolorCount += 2;
                        RotateLeft(x.Parent);
                        x = _root;
                    }
                }
                else
                {
                    RedBlackNode w = x.Parent.Left;
                    if (w.Color == NodeColor.Red)
                    {
                        w.Color = NodeColor.Black;
                        x.Parent.Color = NodeColor.Red;
                        RecolorCount += 2;
                        RotateRight(x.Parent);
                        w = x.Parent.Left;
                    }
                    if (w.Right.Color == NodeColor.Black && w.Left.Color == NodeColor.Black)
                    {
                        w.Color = NodeColor.Red;
                        RecolorCount++;
                        x = x.Parent;
                    }
                    else
                    {
                        if (w.Left.Color == NodeColor.Black)
                        {
                            w.Right.Color = NodeColor.Black;
                            w.Color = NodeColor.Red;
                            RecolorCount += 2;
                            RotateLeft(w);
                            w = x.Parent.Left;
                        }
                        w.Color = x.Parent.Color;
                        x.Parent.Color = NodeColor.Black;
                        w.Left.Color = NodeColor.Black;
                        RecolorCount += 2;
                        RotateRight(x.Parent);
                        x = _root;
                    }
                }
            }
            x.Color = NodeColor.Black;
        }

        private RedBlackNode GetMinimum(RedBlackNode node)
        {
            while (node.Left != _nil)
                node = node.Left;
            return node;
        }

        public bool Search(int key) => SearchNode(_root, key) != _nil;

        private RedBlackNode SearchNode(RedBlackNode node, int key)
        {
            while (node != _nil && key != node.Key)
                node = key < node.Key ? node.Left : node.Right;
            return node;
        }

        public void PrintInOrder()
        {
            if (_root == _nil)
            {
                Console.WriteLine("[Arvore Vazia]");
                return;
            }
            PrintInOrderRec(_root);
            Console.WriteLine();
        }

        private void PrintInOrderRec(RedBlackNode node)
        {
            if (node == _nil) return;
            PrintInOrderRec(node.Left);
            Console.Write($"{node.Key} ");
            PrintInOrderRec(node.Right);
        }

        public void PrintTree()
        {
            if (_root == _nil)
            {
                Console.WriteLine("[Arvore Vazia]");
                return;
            }
            PrintTreeRec(_root, "", true);
        }

        private void PrintTreeRec(RedBlackNode node, string indent, bool last)
        {
            if (node != _nil)
            {
                Console.Write(indent);
                Console.Write(last ? "└── " : "├── ");

                if (node.Color == NodeColor.Red)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{node.Key} [R]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{node.Key} [B]");
                }
                Console.ResetColor();

                indent += last ? "    " : "│   ";
                PrintTreeRec(node.Left, indent, false);
                PrintTreeRec(node.Right, indent, true);
            }
        }
    }
}