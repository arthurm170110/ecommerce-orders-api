# E-commerce Orders API

API REST desenvolvida em **.NET 10** para gerenciamento de pedidos de um e-commerce, seguindo os princípios da **Clean Architecture**, separação de responsabilidades e boas práticas de desenvolvimento.

---

# Objetivo

Disponibilizar uma API para gerenciamento de pedidos contendo as seguintes operações:

* Criar pedido
* Listar pedidos
* Buscar pedido por ID
* Atualizar pedido
* Cancelar pedido

A aplicação utiliza **SQL Server** como banco transacional e **MongoDB** como banco de leitura, implementando uma separação simples entre escrita e consulta.

---

# Tecnologias Utilizadas

* .NET 10
* ASP.NET Core Minimal APIs
* Entity Framework Core
* SQL Server
* MongoDB
* Swagger / OpenAPI
* Memory Cache
* Docker
* Docker Compose
* Clean Architecture

---

# Arquitetura

O projeto está organizado utilizando **Clean Architecture**.

```
src
│
├── EcommerceOrders.Api
│
├── EcommerceOrders.Application
│
├── EcommerceOrders.Domain
│
└── EcommerceOrders.Infrastructure
```

## API

Responsável por:

* Endpoints
* Configuração da aplicação
* Injeção de dependências
* Swagger
* Versionamento

---

## Application

Contém:

* Casos de uso
* Serviços
* DTOs
* Interfaces
* Mapeamentos

---

## Domain

Responsável pelas regras de negócio.

Contém:

* Entidades
* Enums
* Objetos de resultado (`Result<T>`)
* Validações de domínio

---

## Infrastructure

Responsável por:

* Entity Framework Core
* SQL Server
* MongoDB
* Repositórios
* Unit Of Work

---

# Persistência

## SQL Server

Responsável pelas operações de escrita:

* Criar pedidos
* Atualizar pedidos
* Cancelar pedidos

---

## MongoDB

Responsável pelas operações de leitura:

* Buscar pedido
* Listar pedidos

Após alterações no SQL Server, o documento correspondente é sincronizado com o MongoDB.

---

# Cache

A aplicação utiliza **IMemoryCache** para otimizar consultas de pedidos por ID.

Configuração:

* Expiração absoluta: **5 minutos**
* Expiração deslizante: **1 minuto**

Sempre que um pedido é alterado ou cancelado, o cache correspondente é invalidado.

---

# Regras de Negócio

Todo pedido deve possuir:

* Comprador
* Pelo menos um produto

Todo produto deve possuir:

* Nome
* Quantidade
* Preço válido

## Status possíveis

* Started
* Processed
* Shipped
* Cancelled

## Regras

### Atualização

Somente pedidos **Started** podem ser alterados.

### Cancelamento

Somente pedidos:

* Started
* Processed

podem ser cancelados.

### Envio

Somente pedidos **Processed** podem ser enviados.

---

# Base URLs

## Executando com Docker

* **HTTP:** `http://localhost:5000`
* **Swagger:** `http://localhost:5000/swagger`

## Executando localmente

* **HTTP:** `http://localhost:5219`
* **HTTPS:** `https://localhost:7153`

---

# Endpoints

## Criar Pedido

**POST**

### Docker

```
http://localhost:5000/api/v1/orders
```

### Local HTTP

```
http://localhost:5219/api/v1/orders
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders
```

### Request

```json
{
  "buyer": "Arthur Lima",
  "items": [
    {
      "productName": "Notebook Dell",
      "price": 4500.00,
      "quantity": 1
    },
    {
      "productName": "Mouse Logitech",
      "price": 150.00,
      "quantity": 2
    }
  ]
}
```

### Response (201)

```json
{
  "orderId": "56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db",
  "buyer": "Arthur Lima",
  "status": "Started",
  "totalAmount": 4800.00,
  "items": [
    {
      "itemId": "92497e89-54a8-47bc-83fd-64335f7702d8",
      "productName": "Notebook Dell",
      "price": 4500.00,
      "quantity": 1,
      "totalPrice": 4500.00
    },
    {
      "itemId": "8af60bb3-d3fd-4bba-9d57-02f9f0e6a063",
      "productName": "Mouse Logitech",
      "price": 150.00,
      "quantity": 2,
      "totalPrice": 300.00
    }
  ]
}
```

---

## Listar Pedidos

**GET**

### Docker

```
http://localhost:5000/api/v1/orders
```

### Local HTTP

```
http://localhost:5219/api/v1/orders
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders
```

Filtro opcional:

### Docker

```
http://localhost:5000/api/v1/orders?status=Processed
```

### Local HTTP

```
http://localhost:5219/api/v1/orders?status=Processed
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders?status=Processed
```

### Response

```json
[
  {
    "orderId": "56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db",
    "buyer": "Arthur Lima",
    "status": "Processed",
    "totalAmount": 4800.00,
    "items": [
      {
        "itemId": "92497e89-54a8-47bc-83fd-64335f7702d8",
        "productName": "Notebook Dell",
        "price": 4500.00,
        "quantity": 1,
        "totalPrice": 4500.00
      }
    ]
  }
]
```

---

## Buscar Pedido

**GET**

### Docker

```
http://localhost:5000/api/v1/orders/{id}
```

### Local HTTP

```
http://localhost:5219/api/v1/orders/{id}
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders/{id}
```

### Exemplo

### Docker

```
http://localhost:5000/api/v1/orders/56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db
```

### Local HTTP

```
http://localhost:5219/api/v1/orders/56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders/56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db
```

### Response

```json
{
  "orderId": "56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db",
  "buyer": "Arthur Lima",
  "status": "Started",
  "totalAmount": 4800.00,
  "items": [
    {
      "itemId": "92497e89-54a8-47bc-83fd-64335f7702d8",
      "productName": "Notebook Dell",
      "price": 4500.00,
      "quantity": 1,
      "totalPrice": 4500.00
    }
  ]
}
```

---

## Atualizar Pedido

**PUT**

### Docker

```
http://localhost:5000/api/v1/orders/{id}
```

### Local HTTP

```
http://localhost:5219/api/v1/orders/{id}
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders/{id}
```

### Request

```json
{
  "buyer": "Arthur Lima",
  "items": [
    {
      "productName": "Notebook Dell",
      "price": 4300.00,
      "quantity": 1
    },
    {
      "productName": "Teclado Mecânico",
      "price": 400.00,
      "quantity": 1
    }
  ]
}
```

### Response

```json
{
  "orderId": "56b6dcd6-9cc5-4eb1-9bfb-c08fb0ec39db",
  "buyer": "Arthur Lima",
  "status": "Started",
  "totalAmount": 4700.00,
  "items": [
    {
      "itemId": "92497e89-54a8-47bc-83fd-64335f7702d8",
      "productName": "Notebook Dell",
      "price": 4300.00,
      "quantity": 1,
      "totalPrice": 4300.00
    },
    {
      "itemId": "4dce0b4c-5b11-44e3-a2bc-62eb85fdbbb9",
      "productName": "Teclado Mecânico",
      "price": 400.00,
      "quantity": 1,
      "totalPrice": 400.00
    }
  ]
}
```

---

## Cancelar Pedido

**PATCH**

### Docker

```
http://localhost:5000/api/v1/orders/{id}/cancel
```

### Local HTTP

```
http://localhost:5219/api/v1/orders/{id}/cancel
```

### Local HTTPS

```
https://localhost:7153/api/v1/orders/{id}/cancel
```

### Response

```
204 No Content
```

ou

```json
{
  "message": "Order cancelled successfully."
}
```

---

# Tratamento de Erros

A API utiliza o padrão **Result Pattern** para representar sucesso ou falha nas operações, evitando o uso de exceções para controle de fluxo.

Exemplo:

```json
{
  "code": "OrderNotFound",
  "message": "Order not found."
}
```

---

# Logging

A aplicação utiliza `ILogger` para registrar eventos importantes como:

* Criação de pedidos
* Atualização de pedidos
* Cancelamento de pedidos
* Cache Hit / Cache Miss
* Erros inesperados

---

# Executando o Projeto

A forma recomendada de execução é via **Docker**, pois sobe a API e as dependências de infraestrutura de maneira padronizada.

## Executar com Docker

### Subir os containers

```bash
docker compose up --build
```

ou em modo desacoplado:

```bash
docker compose up -d --build
```

### Parar os containers

```bash
docker compose down
```

Após subir os containers, a API estará disponível em:

* `http://localhost:5000`

E o Swagger em:

* `http://localhost:5000/swagger`

---

## Executar localmente

Caso prefira executar sem Docker, será necessário ter os serviços de banco disponíveis localmente.

### Restaurar dependências

```bash
dotnet restore
```

### Executar a aplicação

```bash
dotnet run
```

Após iniciar localmente, a API estará disponível em:

* `http://localhost:5219`
* `https://localhost:7153`

E o Swagger em:

* `http://localhost:5219/swagger`
* `https://localhost:7153/swagger`

---

# Swagger

Após iniciar a aplicação, a documentação estará disponível em:

## Docker

```
http://localhost:5000/swagger
```

## Local HTTP

```
http://localhost:5219/swagger
```

## Local HTTPS

```
https://localhost:7153/swagger
```

---

# Melhorias Futuras

* Testes de integração
* Cache distribuído com Redis
* Mensageria para sincronização entre bancos
* OpenTelemetry
* Serilog
* CI/CD

---

# Autor

Desenvolvido por **Arthur Lima** como solução para o desafio técnico de Desenvolvedor Backend .NET.
