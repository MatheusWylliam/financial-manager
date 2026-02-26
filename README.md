# Financial Manager API

Projeto em desenvolvimento de API RESTful para gerenciamento de objetivos financeiros no modelo de "envelopes" (caixas/metas financeiras), inspirada no sistema de gerenciamento visual do Nubank.

Projeto construído seguindo os princípios de **Clean Architecture**, **CQRS** e **MediatR**, com forte ênfase em separação de responsabilidades e testabilidade.

## Tecnologias Principais

- ASP.NET Core 7
- Clean Architecture (camadas: Core, Application, Infrastructure, API)
- MediatR (mediação de comandos e queries)
- CQRS (separação de escrita e leitura)
- Entity Framework Core
- Repository Pattern + Unit of Work
- FluentValidation (validação de entrada)
- NUnit + Moq (testes unitários)
- AutoMapper

## Estrutura da Solução (Clean Architecture)

| Camada          | Responsabilidade                                                                 | Dependências externas |
|-----------------|----------------------------------------------------------------------------------|------------------------|
| **Core**        | Entidades de domínio, value objects, enums, regras de negócio puras, exceções de domínio | Nenhuma               |
| **Application** | Casos de uso, comandos/queries CQRS, handlers MediatR, DTOs, interfaces de serviço | Core                  |
| **Infrastructure** | Implementações concretas (EF Core repositories, Unit of Work, serviços externos) | Core + Application    |
| **API**         | Controllers, configuração de rotas, middlewares, apresentação de endpoints       | Application + Infrastructure |

## Funcionalidades Implementadas

### Gestão de Caixas / Metas Financeiras

- CRUD completo de caixas (Financial Boxes / Goals)
- Cálculo automático e consistente do saldo atual da caixa
- Suporte a imagem de capa (upload opcional)
- Endpoint de simulação de evolução projetada (projeção de saldo futuro)

### Gestão de Transações

- CRUD completo de transações financeiras
- Validação rigorosa de regras de negócio:
  - Valor > 0
  - Máximo de duas casas decimais
  - Tipos suportados: `Deposit` e `Withdrawal`
- Associação obrigatória a uma caixa/meta
- Atualização transacional do saldo da caixa destino

### Relatórios e Analytics

- Relatório consolidado de evolução por caixa
- Resumo de entradas/saídas por período
- (Extensível) Indicadores avançados de comportamento financeiro

## Regras de Negócio Principais

- Todas as transações monetárias devem possuir **no máximo duas casas decimais**
- Valores monetários **não aceitam sinal negativo** (regra validada em domínio e aplicação)
- Tipos de transação permitidos: `Deposit` (entrada) e `Withdrawal` (saída)
- O saldo de cada caixa é **sempre recalculado** de forma determinística a partir do histórico de transações
- Operações de escrita são atômicas (integridade transacional garantida via Unit of Work)
