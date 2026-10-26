# AmazonLogisticsTrees

**Disciplina:** IMD0039 - Estruturas de Dados Básicas II

**Instituição:** Instituto Metrópole Digital (IMD) - UFRN

**Autor:** Jordson Albino Galvão Tavares

## Contexto do Projeto

Este projeto simula um modelo da Amazon. Atuando como Tech Lead, o objetivo é desenvolver um protótipo para o sistema de logística durante a Black Friday. O sistema precisa registrar, localizar e remover identificadores numéricos de encomendas com extrema eficiência.

Para evitar a degradação de desempenho que ocorre em Árvores de Busca Binária (BST) comuns devido à ordem de chegada dos dados, este protótipo implementa e compara duas estruturas de dados:

1. **Árvore AVL**

2. **Árvore Rubro-Negra**

## Funcionalidades Implementadas

O sistema possui um menu interativo executado via terminal que permite:

* Escolher em tempo de execução qual árvore utilizar.

* **Inserir** identificadores de encomendas (com tratamento contra chaves duplicadas).

* **Remover** identificadores (tratando remoções de folhas, nós com 1 filho, nós com 2 filhos e a raiz).

* **Buscar** encomendas específicas.

* Imprimir o percurso **Em Ordem** (In-Order), exibindo os IDs de forma crescente.

* **Visualizar a estrutura da árvore** hierarquicamente no terminal. (Na Rubro-Negra, exibe as cores vermelho `[R]` e preto `[B]`).

* Executar uma **Bateria de Testes Comparativos Automatizada** que aplica os cenários de pior caso (crescente, decrescente) e casos médios (aleatórios) em ambas as árvores, calculando a altura final, número de rotações e recolorações.

## Decisões de Implementação

Conforme as diretrizes do desafio, as seguintes decisões técnicas foram adotadas:

* **Elementos Repetidos (Duplicatas):** Como os IDs de pacotes são únicos, o sistema bloqueia tentativas de inserir valores já existentes, retornando `false` e exibindo um aviso, mantendo a integridade do sistema.

* **Busca e Remoção de Inexistentes:** Tratados de forma segura sem lançar exceções. Retornam `false` e notificam o usuário amigavelmente de que o pacote não foi encontrado.

* **Nó Sentinela na Árvore Rubro-Negra:** Foi utilizado um nó sentinela único (`_nil`) para representar as folhas nulas. Ele é sempre preto e simplifica a lógica dos algoritmos de rebalanceamento (`InsertFixup` e `DeleteFixup`), prevenindo erros de referência nula (`NullReferenceException`).

* **Fator de Balanceamento AVL:** Calculado dinamicamente através da diferença de altura entre as subárvores esquerda e direita a cada inserção e remoção.

## Como Executar o Projeto

Este projeto foi desenvolvido em **C# (.NET)**. Para executá-lo, você precisará ter o [.NET SDK](https://dotnet.microsoft.com/download) instalado na sua máquina (versão 10.0 recomendada).

### Passo a passo:

1. **Clone o repositório:**

   ```
   git clone https://github.com/JordsonZ/AmazonLogisticsTrees.git
   
   ```

2. **Acesse a pasta do projeto:**

   ```
   cd .\AmazonLogisticsTrees\
   
   ```

3. **Compile e execute a aplicação:**

   ```
   dotnet run
   
   ```

4. **Navegue pelo Menu Interativo:**
   Siga as instruções exibidas no console para inserir dados manualmente ou utilize a opção **3** do menu principal para rodar automáticamente os testes da Black Friday.
