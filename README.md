# Desafio-BackEnd

Repositório com a resolução do desafio para a vaga de desenvolvedor backend dotnet

## Aplicação a ser desenvolvida

Seu objetivo é criar uma aplicação para gerenciar aluguel de motos e entregadores. Quando um entregador estiver registrado e com uma locação ativa poderá também efetuar entregas de pedidos disponíveis na plataforma.

## Tecnologias e bibliotecas utilizadas

- ASP.Net Core 8.0
- Postegresql
- RabbitMQ
- Azure Blob Storage
- Entity Framework Core
- Carter
- Serilog
- CQRS (Command Query Responsibility Segregation)
- MediatR
- MassTransit
- Fluent Validation
- Automapper
- Docker e Docker-Compose
- XUnit
- Fluent Assertions
- Moq

## Arquitetura e Modelagem de Dados

Optei por utilizar a arquitetura baseado no conceito de Clean Architecture, ficou organizado da seguinte maneira:

```
├── source
│   ├── DesafioBackEnd.Application
│   ├── DesafioBackEnd.Domain
│   ├── DesafioBackEnd.Infrastructure
│   └── DesafioBackEnd.WebApi
│       └── Program.cs
├── tests
│   └── DesafioBackEnd.UnitTests
├── .dockerignore
├── .gitignore
├── DesafioBackEnd.sln
├── docker-compose.yml
├── launchSettings.json
└── README.md
```

Modelagem da base de dados:

![Diagrama Entidade Relacionamento (DER)](https://github.com/ThallesTeodoro/Desafio-BackEnd/blob/development/der.png?raw=true)

## Setup da Aplicação

A aplicação utiliza Docker e Docker Compose. Com isso, para executar a aplicação bata tê-los instalados e seguir as orientações:

### Requisitos

- Docker
- Docker Compose

### Executar o comando

```bash
docker-compose up -d
```

O comando irá subir 4 containers, sendo eles:

- desafiobackend.webapi
- desafiobackend.database
- desafiobackend.rabbitmq
- desafiobackend.blob-storage

Fique a vontade para explorar e entender mais sobre os containers visualizando o arquivo docker-compose.yml


## Utilização da Aplicação

A aplicação tem autenticação e autorização, feita de forma bem básica. Por esse motivo é necessário se autenticar utilizando o endpoint /login informando o e-mail.

```
http://localhost5000/login
```

Esse endpoint, caso o e-mail exista, retornará o token de autenticação.

```bash
{
    "statusCode": 200,
    "message": "OK",
    "data": {
        "type": "Bearer",
        "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJlNTc1NzM5NS1kOGIxLTRlNTYtYjRmYS1iNjhmZjA4OTU0N2IiLCJlbWFpbCI6ImFkbWluQGRlc2FmaW9iYWNrZW5kLmNvbSIsImV4cCI6MTcxNjI0MzI0OSwiaXNzIjoiRGVzYWZpb0JhY2tFbmQiLCJhdWQiOiJEZXNhZmlvQmFja0VuZCJ9.g2yOgLino06jxlTggBLCa0XKkALabICyMJ-lXR46aE8"
    },
    "errors": null
}
```

Esse token deve ser utilizado nas requisições para se autenticar.

** Dica: Se desejar, neste repositório há um arquivo chamado "DesafioBackEnd.postman_collection.json", você pode importa-lo no Postman para ter uma base das requisições. Caso não queira seguir essa abordagem, pode também utilizar do Swagger da aplicação, disponível no link http://localhost:5000/swagger.


### Tipos e Usuário e Níveis de Permissões

Existem 2 tipos de usuários na aplicação: Administrador e Entregador. Cada usuário tem suas permissões dentro da aplicação. Permissões de gerenciamento são dadas ao Administrador. Ao Entregador, é permitido se cadastrar, alugar uma moto e realizar entregas.

** Importante: usuários administradores não podem se cadastrar no sistema, por esse motivo já há um usuário administrador cadastrado para que possa realizar os testes com esse tipo de usuário. Utilize o email: admin@desafiobackend.com

### Base de dados

Para a atualização da base dados foi utilizado o conceito de Migrations. Elas vão rodar ao startar a aplicação no ambiente de Desenvolvimento e dentro do contexto do Docker, juntamente com os Seeds que vão popular a base de dados rom os Perfis, Permissões, Planos e Usuários, dados fixos no sistema e não há endpoint para gerencia-los.

### Storage

As imagens são salvas no storage local que funciona através do container docker "desafiobackend.blob-storage".

---

Para mais informações a respeito do projeto, entre em contato através do e-mail ou Whatsapp.
