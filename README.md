# Digital Innovation One - Prática ASP.NET Core

## Criando um catálogo de jogos usando boas práticas de arquitetura com .NET

Projeto desenvolvido durante o bootcamp TakeBlip Fullstack Developer #2 na plataforma DIO com o intuito de aplicar boas práticas de arquitetura implementando-os utilizando ASP.NET Core.
Consiste em uma RESTful API que permite simula um catálogo de jogos.

Em relação ao projeto base:
- Aumentada a abstração das camadas Service e Repository
- Simplificado processo de injeção de dependências utilizando Reflexão
- Adicionadas entidades:
  - Produtora (Relação 1xN com Jogo e 1xN com Produtora)
  - Usuário (Relação NxN com Jogo utilizando entidade composta UsuarioJogo)
- Adicionado processo de autentição Jwt Bearer e Autorização baseada em nível de Permissão de Usuários
- Modificado Middleware customizado para tratamento de exceções
- Adicionados testes unitários para as camadas de Controller e Service


## Créditos da versão original:

Thiago Campos de Oliveira: https://www.linkedin.com/in/thiago-campos-de-oliveira-693a3840/
