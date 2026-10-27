using System;
using AmazonLogisticsTrees.Interfaces;
using AmazonLogisticsTrees.Trees;

namespace AmazonLogisticsTrees.Services
{
    public static class TestRunner
    {
        public static void RunComparativeSuite()
        {
            Console.Clear();
            Console.WriteLine("Amazon Logistics - tests (AVL vs RN) \n");

            int[] ascending = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            int[] descending = { 100, 90, 80, 70, 60, 50, 40, 30, 20, 10 };
            int[] unordered = { 55, 20, 80, 10, 35, 65, 95, 5, 15, 30, 45, 60, 75, 85, 110 };

            RunScenario("Cenário 1: Inserção em Ordem Crescente (10 a 100)", ascending);
            RunScenario("Cenário 2: Inserção em Ordem Decrescente (100 a 10)", descending);
            RunScenario("Cenário 3: Inserção Não Ordenada (15 encomendas)", unordered);

            RunEdgeCasesAndRemovals();
        }

        private static void RunScenario(string title, int[] dataset)
        {
            AvlTree avl = new AvlTree();
            RedBlackTree rbt = new RedBlackTree();

            foreach (int item in dataset)
            {
                avl.Insert(item);
                rbt.Insert(item);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{title}");
            Console.ResetColor();
            Console.WriteLine($"Elementos inseridos: {string.Join(", ", dataset)}");
            Console.WriteLine($"{"Métrica",-25} | {"Árvore AVL",-15} | {"Árvore Rubro-Negra",-18}");
            Console.WriteLine(new string('-', 65));
            Console.WriteLine($"{"Altura Final",-25} | {avl.GetHeight(),-15} | {rbt.GetHeight(),-18}");
            Console.WriteLine($"{"Total de Rotações",-25} | {avl.RotationCount,-15} | {rbt.RotationCount,-18}");
            Console.WriteLine($"{"Ajustes de Cor (RN)",-25} | {"N/A",-15} | {rbt.RecolorCount,-18}\n");

            Console.WriteLine(">> Estrutura Visual Final (AVL):");
            avl.PrintTree();
            Console.WriteLine("\n>> Estrutura Visual Final (Rubro-Negra):");
            rbt.PrintTree();
        }

        private static void RunEdgeCasesAndRemovals()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Cenário 4: Casos de Borda, Duplicatas e Remoções (0, 1 e 2 filhos)");
            Console.ResetColor();

            AvlTree avl = new AvlTree();
            RedBlackTree rbt = new RedBlackTree();

            Console.WriteLine("1. Operação em árvore vazia (Remover ID 999):");
            Console.WriteLine($"   AVL retornou: {avl.Remove(999)} | RN retornou: {rbt.Remove(999)}");

            Console.WriteLine("2. Árvore com apenas 1 elemento (Inserir e Remover ID 500):");
            avl.Insert(500); rbt.Insert(500);
            Console.WriteLine($"Altura c/ 1 elem -> AVL: {avl.GetHeight()} | RN: {rbt.GetHeight()}");
            avl.Remove(500); rbt.Remove(500);
            Console.WriteLine($"Altura após remoção -> AVL: {avl.GetHeight()} | RN: {rbt.GetHeight()}");

            int[] baseData = { 50, 30, 70, 20, 40, 60, 80, 15 };
            foreach (int id in baseData) { avl.Insert(id); rbt.Insert(id); }

            Console.WriteLine("\n3. Tentativa de inserir duplicata (ID 50):");
            Console.WriteLine($"AVL aceitou? {avl.Insert(50)} | RN aceitou? {rbt.Insert(50)}");

            Console.WriteLine("\n4. Busca de existente (40) e inexistente (999):");
            Console.WriteLine($"Busca 40  -> AVL: {avl.Search(40)} | RN: {rbt.Search(40)}");
            Console.WriteLine($"Busca 999 -> AVL: {avl.Search(999)} | RN: {rbt.Search(999)}");

            Console.WriteLine("\n5. Remoções com diferentes configurações de filhos:");
            Console.WriteLine("Removendo folha (80), nó com 1 filho (20) e nó com 2 filhos/raiz (50)");
            avl.Remove(80); rbt.Remove(80);
            avl.Remove(20); rbt.Remove(20);
            avl.Remove(50); rbt.Remove(50);

            Console.WriteLine($"Métricas Pós-Remoções -> AVL (Altura: {avl.GetHeight()}, Rotações: {avl.RotationCount}) | RN (Altura: {rbt.GetHeight()}, Rotações: {rbt.RotationCount})");
            Console.Write("Percurso em Ordem (AVL): "); avl.PrintInOrder();
            Console.Write("Percurso em Ordem (RN): "); rbt.PrintInOrder();
            Console.WriteLine();
        }
    }
}