# AcademicSupportQueueApi

## Integrantes

* Ranielly Fernandes
* Ana Caroline
* Scott Iwano

## Descrição do Projeto

O AcademicSupportQueueApi é uma API desenvolvida para gerenciar solicitações de suporte acadêmico e monitoria. O sistema organiza os atendimentos utilizando uma fila de prioridade, permitindo que solicitações mais urgentes sejam atendidas primeiro.

A prioridade é calculada automaticamente com base no tipo da solicitação e na data de entrega, garantindo que alunos com demandas mais urgentes recebam atendimento prioritário.

---

# Tecnologias Utilizadas

* ASP.NET Core 8
* C#
* Entity Framework Core
* SQL Server
* Docker
* Swagger/OpenAPI
* xUnit
* GitHub

---

# Arquitetura do Projeto

O projeto foi desenvolvido utilizando uma arquitetura em camadas:

```text
AcademicSupportQueueApi
│
├── Api
│   └── Controllers
│
├── Domain
│   ├── Entidades
│   ├── Interfaces
│   ├── Services
│   └── PriorityRules
│
├── Infrastructure
│   ├── Dados
│   ├── Migrations
│   └── Repositories
│
├── Application
│
└── UnitTests
```

---

# Regra de Prioridade

A prioridade das solicitações é calculada automaticamente pelo sistema utilizando:

* Tipo da solicitação
* Data de entrega

Quanto mais urgente for a solicitação, maior será sua prioridade.

Em caso de empate, a solicitação cadastrada primeiro será atendida antes das demais.

---

# Estrutura de Dados Utilizada

O sistema utiliza Heap (fila de prioridade) para organizar os atendimentos.

Essa estrutura permite que a solicitação mais prioritária seja localizada rapidamente, tornando o atendimento mais eficiente.

---

# Exclusão Lógica

O sistema não remove registros fisicamente do banco de dados.

Ao excluir uma solicitação:

* O status é alterado para "Excluido"
* A data de exclusão é registrada

Dessa forma o histórico é preservado para futuras consultas.

---

# Banco de Dados

Banco utilizado:

* SQL Server 2022

A comunicação entre a aplicação e o banco é realizada através do Entity Framework Core.

---

# Executando com Docker

Na raiz do projeto execute:

```bash
docker compose up -d
```

Verifique se o container está ativo:

```bash
docker ps
```

---

# Aplicando as Migrations

Após iniciar o banco execute:

```bash
dotnet ef database update --project src\AcademicSupportQueueApi.Infrastructure --startup-project src\AcademicSupportQueueApi.Api
```

---

# Executando a Aplicação

```bash
dotnet run --project src\AcademicSupportQueueApi.Api
```

---

# Swagger

Após executar a aplicação, acesse:

```text
https://localhost:7245/swagger
```

ou

```text
http://localhost:5186/swagger
```

O Swagger permite visualizar e testar todos os endpoints da API.

---

# Endpoints Disponíveis

## Solicitações Acadêmicas

### Listar solicitações

```http
GET /solicitacoes-academicas
```

### Buscar por ID

```http
GET /solicitacoes-academicas/{id}
```

### Buscar por descrição

```http
GET /solicitacoes-academicas/buscar
```

### Cadastrar solicitação

```http
POST /solicitacoes-academicas
```

### Atualizar solicitação

```http
PUT /solicitacoes-academicas/{id}
```

### Atualizar status

```http
PATCH /solicitacoes-academicas/{id}/status
```

### Excluir solicitação

```http
DELETE /solicitacoes-academicas/{id}
```

### Consultar próximo atendimento

```http
GET /solicitacoes-academicas/proximo
```

### Atender próximo da fila

```http
POST /solicitacoes-academicas/proximo/atender
```

### Estatísticas

```http
GET /solicitacoes-academicas/estatisticas
```

---


# Resultado

O projeto entrega uma API completa para gerenciamento de suporte acadêmico, utilizando fila de prioridade baseada em Heap, banco de dados SQL Server, Docker para execução do ambiente e Swagger para documentação e testes dos endpoints.
