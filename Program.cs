using System;
using AmazonLogisticsTrees.Interfaces;
using AmazonLogisticsTrees.Services;
using AmazonLogisticsTrees.Trees;

namespace AmazonLogisticsTrees
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("  AMAZON LOGISTICS - SISTEMA DE PACOTES (BLACK FRIDAY)   ");
                Console.WriteLine("Escolha a estrutura de dados para operar:");
                Console.WriteLine("1 - Árvore AVL");
                Console.WriteLine("2 - Árvore Rubro-Negra");
                Console.WriteLine("3 - Executar Testes Comparativos (Automatico)");
                Console.WriteLine("0 - Sair");
                Console.Write("\nOpção: ");

                string? choice = Console.ReadLine();

                if (choice == "0") break;
                if (choice == "3")
                {
                    TestRunner.RunComparativeSuite();
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu");
                    Console.ReadKey();
                    continue;
                }

                IBalancedTree? tree = choice switch
                {
                    "1" => new AvlTree(),
                    "2" => new RedBlackTree(),
                    _ => null
                };

                if (tree == null)
                {
                    Console.WriteLine("Opção inválida! Pressione qualquer tecla");
                    Console.ReadKey();
                    continue;
                }

                OperateTreeMenu(tree);
            }
        }

        static void OperateTreeMenu(IBalancedTree tree)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"OPERANDO: {tree.TreeName.ToUpper()}");
                Console.WriteLine($"Altura Atual: {tree.GetHeight()} | Total de Rotações: {tree.RotationCount}");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("1 - Inserir Encomenda (ID)");
                Console.WriteLine("2 - Remover Encomenda (ID)");
                Console.WriteLine("3 - Buscar Encomenda (ID)");
                Console.WriteLine("4 - Listar em Ordem Crescente (In-Order)");
                Console.WriteLine("5 - Visualizar Estrutura da Árvore");
                Console.WriteLine("6 - Inserir Lote de Encomendas (separados por vírgula)");
                Console.WriteLine("0 - Voltar ao Menu Principal");
                Console.Write("\nEscolha uma ação: ");

                string? op = Console.ReadLine();
                if (op == "0") break;

                Console.WriteLine();
                switch (op)
                {
                    case "1":
                        Console.Write("Digite o ID da encomenda para INSERIR: ");
                        if (int.TryParse(Console.ReadLine(), out int insId))
                        {
                            if (tree.Insert(insId))
                                Console.WriteLine($"[SUCESSO] Encomenda {insId} registrada!");
                            else
                                Console.WriteLine($"[AVISO] Encomenda {insId} já existe no sistema (duplicata ignorada).");
                        }
                        else Console.WriteLine("[ERRO] Digite um número inteiro válido.");
                        break;

                    case "2":
                        Console.Write("Digite o ID da encomenda para REMOVER: ");
                        if (int.TryParse(Console.ReadLine(), out int remId))
                        {
                            if (tree.Remove(remId))
                                Console.WriteLine($"[SUCESSO] Encomenda {remId} removida com sucesso!");
                            else
                                Console.WriteLine($"[ERRO] Encomenda {remId} não encontrada no sistema.");
                        }
                        else Console.WriteLine("[ERRO] Digite um número inteiro válido.");
                        break;

                    case "3":
                        Console.Write("Digite o ID da encomenda para BUSCAR: ");
                        if (int.TryParse(Console.ReadLine(), out int searchId))
                        {
                            bool found = tree.Search(searchId);
                            Console.WriteLine(found
                                ? $"[ENCONTRADO] A encomenda {searchId} está registrada na árvore."
                                : $"[NÃO ENCONTRADO] A encomenda {searchId} não consta no sistema.");
                        }
                        else Console.WriteLine("[ERRO] Digite um número inteiro válido.");
                        break;

                    case "4":
                        Console.WriteLine("Encomendas em Ordem Crescente:");
                        tree.PrintInOrder();
                        break;

                    case "5":
                        Console.WriteLine("Estrutura Hierárquica Atual:");
                        tree.PrintTree();
                        break;

                    case "6":
                        Console.Write("Digite os IDs separados por vírgula (ex: 10, 20, 30): ");
                        string? batch = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(batch))
                        {
                            foreach (var part in batch.Split(','))
                            {
                                if (int.TryParse(part.Trim(), out int id))
                                {
                                    bool ok = tree.Insert(id);
                                    Console.WriteLine(ok ? $"  + {id} inserido." : $"  ! {id} ignorado (repetido).");
                                }
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                Console.WriteLine("\nPressione qualquer tecla para continuar");
                Console.ReadKey();
            }
        }
    }
}