# CepAPI

Desafio BackEnd da NegocieOnline para criar uma API de consulta de Cep.

## Tecnologias Usadas

- .NET 8
- ASP.NET Core
- EntityFrameworkCore
- Swagger
- AutoMapper
- FluentValidation
- PostgreSQL
- ViaCepAPI

## Observações

Não consegui realizar o update da database do banco, por isso deixei um script sql gerado pelo EntityFramework que pode ser usado para a geração da tabela pelo DBA responsável.

## Endpoints

- `POST` /post-cep: Consulta o Cep informado através do RequestBody, e em seu ResponseBody retorna os dados de logradouro, cidade, estado e bairro. Caso o Cep não exista, retorna que não foi encontrado pelo serviço externo.
- `GET` /get-cep/{cep}: Retorna as informações do cep armazenado no banco de dados. Caso não for encontrado, retorna que não consta.
