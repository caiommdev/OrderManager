# Estrutura de Camadas - Clean Architecture

## 📐 Arquitetura de Camadas Implementada

Este documento detalha a organização das camadas seguindo os princípios de **Clean Architecture** de Robert C. Martin.

### 🎯 Princípios Fundamentais

1. **Dependency Rule**: Dependências apontam sempre para o centro (Domain)
2. **Independence**: Cada camada é independente e testável
3. **Separation of Concerns**: Cada camada tem responsabilidades específicas
4. **Abstraction**: Camadas externas dependem de abstrações, não implementações

## 🏗️ Estrutura Detalhada das Camadas

### 🔵 Domain (Camada de Domínio)
**Namespace**: `OrderManager.API.Domain.*`  
**Responsabilidade**: Regras de negócio puras, independentes de tecnologia

```
📁 Domain/
├── 📁 Entities/
│   └── 📄 Order.cs                           # Entidade principal com regras de negócio
├── 📁 ValueObjects/
│   ├── 📄 Weight.cs                          # Peso (sem validação interna)
│   ├── 📄 Address.cs                         # Endereço (apenas dados)
│   └── 📄 Recipient.cs                       # Destinatário (apenas dados)
├── 📁 Interfaces/
│   └── 📄 IShippingCalculator.cs             # Contrato para cálculo de frete
├── 📁 Services/
│   ├── 📄 ExpressShippingCalculator.cs       # Estratégia frete expresso
│   ├── 📄 StandardShippingCalculator.cs      # Estratégia frete padrão
│   └── 📄 EconomyShippingCalculator.cs       # Estratégia frete econômico
├── 📁 Exceptions/
│   ├── 📄 DomainException.cs                 # Exceção base do domínio
│   ├── 📄 InvalidWeightException.cs          # Exceção peso inválido
│   ├── 📄 InvalidAddressException.cs         # Exceção endereço inválido
│   ├── 📄 InvalidRecipientException.cs       # Exceção destinatário inválido
│   └── 📄 UnsupportedShippingTypeException.cs # Exceção tipo frete inválido
└── 📁 Enums/
    └── 📄 ShippingType.cs                    # Enumeração tipos de frete
```

**Características:**
- ✅ Não tem dependências externas
- ✅ Contém regras de negócio puras
- ✅ Value Objects são imutáveis e sem lógica
- ✅ Interfaces definem contratos do domínio

### 🟢 Application (Camada de Aplicação)
**Namespace**: `OrderManager.API.Application.*`  
**Responsabilidade**: Casos de uso e orquestração de operações

```
📁 Application/
└── 📁 Services/
    ├── 📄 DeliveryService.cs                 # Orquestração criação de entregas
    ├── 📄 LabelService.cs                    # Geração de etiquetas e resumos
    ├── 📁 Factories/
    │   └── 📄 ShippingCalculatorFactory.cs   # Factory para calculadoras
    ├── 📁 Interfaces/
    │   ├── 📄 IDeliveryService.cs            # Interface serviço entrega
    │   ├── 📄 ILabelService.cs               # Interface serviço etiquetas
    │   ├── 📁 Factories/
    │   │   └── 📄 IShippingCalculatorFactory.cs # Interface factory
    │   └── 📁 Validation/
    │       └── 📄 IValidationService.cs      # Interface validação
    └── 📁 Validation/
        └── 📄 ValidationService.cs           # Implementação validações
```

**Características:**
- ✅ Depende apenas do Domain
- ✅ Contém casos de uso (use cases)
- ✅ Orquestra operações do domínio
- ✅ Isola o domínio de detalhes externos

### 🟡 Presentation (Camada de Apresentação)
**Namespace**: `OrderManager.API.Controllers.*` e `OrderManager.API.Contracts.*`  
**Responsabilidade**: Interface com o mundo externo (APIs, UI)

```
📁 Presentation/
├── 📁 Controllers/
│   └── 📄 DeliveryController.cs              # Endpoints da API REST
└── 📁 Contracts/
    ├── 📁 Requests/
    │   └── 📄 CreateDeliveryRequest.cs       # DTO entrada
    └── 📁 Responses/
        ├── 📄 DeliveryResponse.cs            # DTO saída entrega
        └── 📄 PromotionalDiscountResponse.cs # DTO saída desconto
```

**Características:**
- ✅ Depende de Application e Domain (via DI)
- ✅ Transforma DTOs em comandos de domínio
- ✅ Trata aspectos HTTP específicos
- ✅ Serialização/Deserialização de dados

### 🔴 Infrastructure (Camada de Infraestrutura)
**Namespace**: `OrderManager.API.Infrastructure.*`  
**Responsabilidade**: Detalhes técnicos e dependências externas

```
📁 Infrastructure/
├── 📁 Configuration/
│   └── 📄 DependencyInjection.cs            # Configuração container DI
└── 📁 CrossCutting/
    └── 📄 Extensions.cs                      # Extensões úteis
```

**Características:**
- ✅ Implementa interfaces do Application/Domain
- ✅ Contém detalhes de frameworks
- ✅ Configurações de banco de dados, APIs externas
- ✅ Logging, caching, messaging

## 🔄 Fluxo de Dependências

```
┌─────────────────┐
│   Presentation  │ ──┐
└─────────────────┘   │
                      ▼
┌─────────────────┐ ┌─────────────────┐
│ Infrastructure  │ │   Application   │
└─────────────────┘ └─────────────────┘
          │                   │
          └─────────┬─────────┘
                    ▼
          ┌─────────────────┐
          │     Domain      │ ◄── Centro da Arquitetura
          └─────────────────┘
```

**Regra de Dependência:**
- **Presentation** → **Application** → **Domain**
- **Infrastructure** → **Application** → **Domain**
- **Domain** não depende de ninguém (núcleo limpo)

## ✨ Benefícios da Estrutura

### 🧪 Testabilidade
- **Unit Tests**: Domain isolado, 100% testável
- **Integration Tests**: Application com mocks de Infrastructure
- **E2E Tests**: Presentation com toda stack

### 🔧 Manutenibilidade
- **Mudanças no UI**: Apenas Presentation afetada
- **Mudanças no Banco**: Apenas Infrastructure afetada
- **Mudanças de Negócio**: Apenas Domain/Application afetadas

### 🚀 Extensibilidade
- **Novos Endpoints**: Adicionar em Presentation
- **Novos Casos de Uso**: Adicionar em Application
- **Novas Regras**: Adicionar em Domain
- **Novas Tecnologias**: Adicionar em Infrastructure

### 💡 Exemplo Prático de Extensão

**Cenário**: Adicionar cálculo de frete premium

1. **Domain**: Criar `PremiumShippingCalculator : IShippingCalculator`
2. **Application**: Atualizar `ShippingCalculatorFactory`
3. **Domain**: Adicionar `Premium` no enum `ShippingType`
4. **Presentation**: Nenhuma mudança necessária!

## 🎯 Conclusão

A estrutura de camadas implementada garante:

- ✅ **Código limpo e organizando**
- ✅ **Fácil manutenção e evolução**
- ✅ **Alta testabilidade**
- ✅ **Baixo acoplamento**
- ✅ **Alta coesão**
- ✅ **Princípios SOLID respeitados**

Esta arquitetura permite que o sistema evolua de forma sustentável, mantendo a qualidade e facilitando o desenvolvimento de novos recursos.