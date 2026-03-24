# Expense Tracker

Aplicacao de linha de comando desenvolvida em .NET 8 para registrar, listar, atualizar, excluir, resumir e exportar despesas pessoais.

O projeto segue a ideia de um gerenciador simples de gastos, com foco em praticar manipulacao de argumentos via terminal, persistencia local em arquivo JSON e exportacao para CSV.

## Objetivo

Este projeto permite controlar despesas diretamente pelo terminal, sem banco de dados e sem interface grafica. Cada despesa recebe um identificador unico, uma descricao, um valor e a data em que foi criada.

## Funcionalidades

- Adicionar uma nova despesa
- Listar todas as despesas cadastradas
- Atualizar descricao e/ou valor de uma despesa existente
- Excluir uma despesa pelo ID
- Exibir o total geral das despesas
- Exibir o total das despesas de um mes especifico
- Exportar os registros para um arquivo CSV

## Tecnologias Utilizadas

- C#
- .NET 8
- System.Text.Json para serializacao dos dados
- Console Application (CLI)

## Estrutura do Projeto

```text
Expense-Tracker/
|-- README.md
`-- ExpenseTracker/
    |-- Program.cs
    |-- ExpenseTracker.csproj
    |-- ExpenseTracker.sln
    |-- Models/
    |   `-- ExpenseModel.cs
    |-- Services/
    |   `-- ExpenseService.cs
    |-- Properties/
    |   `-- launchSettings.json
    |-- ExpenseTracker.json
    `-- expenses.csv
```

## Como Funciona

O ponto de entrada da aplicacao fica em `Program.cs`, que interpreta os argumentos enviados no terminal e direciona a execucao para o `ExpenseService`.

O `ExpenseService` concentra as regras principais:

- leitura e escrita no arquivo `ExpenseTracker.json`
- validacao basica dos dados
- geracao automatica de IDs
- calculo de totais
- exportacao para `expenses.csv`

Cada registro usa o modelo `ExpenseModel`, com os seguintes campos:

- `Id`
- `Date`
- `Description`
- `Amount`

## Requisitos

Antes de executar o projeto, voce precisa ter instalado:

- .NET SDK 8.0 ou superior

Para verificar:

```bash
dotnet --version
```

## Como Executar

Entre na pasta do projeto onde esta o arquivo `.csproj`:

```bash
cd ExpenseTracker
```

Depois execute um dos comandos abaixo:

```bash
dotnet run -- add --description "Lunch" --amount 20
dotnet run -- list
dotnet run -- summary
dotnet run -- summary --month 3
dotnet run -- update --id 1 --description "Lunch at work" --amount 25
dotnet run -- delete --id 1
dotnet run -- export
```

## Comandos Disponiveis

### `add`

Adiciona uma nova despesa.

```bash
dotnet run -- add --description "Internet" --amount 99.90
```

Saida esperada:

```text
Expense added successfully (ID: 1)
```

### `list`

Lista todas as despesas cadastradas.

```bash
dotnet run -- list
```

Exemplo de saida:

```text
ID  Date        Description      Amount
1   2026-03-24  Lunch            $20
2   2026-03-24  Dinner           $10
```

### `summary`

Exibe a soma total de todas as despesas.

```bash
dotnet run -- summary
```

Exemplo de saida:

```text
Total expenses: $30
```

### `summary --month`

Exibe a soma das despesas de um mes especifico do ano atual.

```bash
dotnet run -- summary --month 3
```

Exemplo de saida:

```text
Total expenses for March: $30
```

### `update`

Atualiza uma despesa pelo ID. E possivel alterar apenas a descricao, apenas o valor ou ambos.

```bash
dotnet run -- update --id 1 --description "Lunch at work" --amount 25
```

Saida esperada:

```text
Expense 1 updated successfully.
```

### `delete`

Remove uma despesa pelo ID.

```bash
dotnet run -- delete --id 1
```

Saida esperada:

```text
Expense 1 deleted successfully.
```

### `export`

Exporta todas as despesas para um arquivo CSV.

```bash
dotnet run -- export
```

Saida esperada:

```text
Expenses exported successfully to: caminho/do/arquivo/expenses.csv
```

## Persistencia dos Dados

As despesas sao armazenadas localmente em um arquivo JSON chamado `ExpenseTracker.json`.

Exemplo de estrutura:

```json
[
  {
    "Id": 1,
    "Date": "2026-03-24T13:11:53.3405878-03:00",
    "Description": "Lunch",
    "Amount": 20
  }
]
```

Quando o comando `export` e executado, a aplicacao gera tambem um arquivo `expenses.csv` com o seguinte formato:

```csv
Id,Date,Description,Amount
1,2026-03-24,Lunch,20
```

## Validacoes Implementadas

Atualmente o projeto contem as seguintes validacoes:

- a descricao nao pode ser vazia
- o valor da despesa deve ser maior que zero
- o ID informado em `update` e `delete` deve ser valido
- o mes informado em `summary --month` deve estar entre `1` e `12`
- em `update`, pelo menos um campo precisa ser informado para alteracao

## Observacoes Importantes

- Os arquivos `ExpenseTracker.json` e `expenses.csv` sao criados no diretorio atual de execucao da aplicacao.
- Por isso, o ideal e executar os comandos dentro da pasta `ExpenseTracker`, para manter os arquivos de dados junto do projeto.
- O comando `summary --month` considera apenas despesas do ano atual.
- O projeto utiliza `DateTime.Now`, entao a data da despesa e registrada no momento em que ela e criada.
- O `launchSettings.json` esta configurado com `list` como argumento padrao ao executar pelo perfil do projeto em algumas IDEs.

## Exemplo de Fluxo de Uso

```bash
cd ExpenseTracker
dotnet run -- add --description "Lunch" --amount 20
dotnet run -- add --description "Dinner" --amount 10
dotnet run -- list
dotnet run -- summary
dotnet run -- update --id 1 --description "Lunch at work" --amount 25
dotnet run -- export
```

## Melhorias Futuras

- adicionar testes automatizados
- permitir filtros por intervalo de datas
- melhorar a formatacao da listagem no terminal
- permitir configuracao do caminho do arquivo de dados
- adicionar categorias para as despesas
- separar melhor regras de negocio e camada de persistencia

## Status do Projeto

Projeto funcional para estudo de C# com foco em aplicacoes CLI e manipulacao de arquivos locais.

## Referencia do desafio

Este projeto foi desenvolvido com base no desafio proposto pelo roadmap.sh:

https://roadmap.sh/projects/expense-tracker
