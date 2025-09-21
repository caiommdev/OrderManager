# Relatório Técnico - Refatoração do Sistema de Pedidos

## 1. Abstrações e Separação em Camadas

### Estrutura de Camadas Implementada

A refatoração seguiu uma arquitetura em camadas bem definida:

**Camada de Domínio (`Domain/`)**
**Camada de Domínio (`Domain/`)**
- **Entidades**: `Order` - Representa o conceito central de uma entrega
- **Value Objects**: `Weight`, `Address`, `Recipient` - Encapsulam valores sem lógica
- **Interfaces**: `IShippingCalculator` - Define contratos do domínio
- **Serviços de Domínio**: ExpressShippingCalculator, StandardShippingCalculator, EconomyShippingCalculator
- **Exceções**: Exceções específicas do domínio

**Camada de Aplicação (`Application/`)**
- **DeliveryService**: Orquestra a criação de entregas (casos de uso)
- **LabelService**: Orquestra a geração de etiquetas e resumos
- **ValidationService**: Centraliza validações de entrada
- **ShippingCalculatorFactory**: Cria instâncias das estratégias de cálculo

**Camada de Apresentação (`Presentation/`)**
- **DeliveryController**: Expõe a API REST e trata requisições HTTP
- **Contracts**: DTOs para comunicação externa (Requests/Responses)


### Exemplo de Abstração - Strategy Pattern

```csharp
// Interface que define o contrato
public interface IShippingCalculator
{
    decimal CalculateShippingCost(Weight weight);
    bool IsEligibleForFreeShipping(Weight weight);
    string GetShippingTypeName();
}

// Implementação específica para frete econômico
public class EconomyShippingCalculator : IShippingCalculator
{
    private const decimal WeightMultiplier = 1.1m;
    private const decimal Discount = 5m;
    private const double FreeShippingWeightLimit = 2.0;

    public decimal CalculateShippingCost(Weight weight)
    {
        if (IsEligibleForFreeShipping(weight))
            return 0m;

        var cost = (decimal)weight.Value * WeightMultiplier - Discount;
        return Math.Max(cost, 0m);
    }
    
    // ...demais métodos
}
```

**Benefícios da Abstração:**
- **Extensibilidade**: Novos tipos de frete podem ser adicionados sem modificar código existente
- **Testabilidade**: Cada estratégia pode ser testada isoladamente
- **Manutenibilidade**: Mudanças em um tipo de frete não afetam outros
- **Eliminação de If-Else**: Substitui a estrutura rígida por polimorfismo

### Factory Pattern para Criação de Estratégias

```csharp
public class ShippingCalculatorFactory : IShippingCalculatorFactory
{
    public IShippingCalculator CreateCalculator(ShippingType shippingType)
    {
        return shippingType switch
        {
            ShippingType.Express => new ExpressShippingCalculator(),
            ShippingType.Standard => new StandardShippingCalculator(), 
            ShippingType.Economy => new EconomyShippingCalculator(),
            _ => throw new UnsupportedShippingTypeException(shippingType.ToString())
        };
    }
}
```

## 2. Contratos e Validação de Integridade

### Value Objects com Validação Intrínseca

Implementamos Value Objects que garantem estados válidos desde a criação:

```csharp
public record Weight
{
    public double Value { get; }

    public Weight(double value)
    {
        if (value <= 0)
            throw new InvalidWeightException(value);

        Value = value;
    }
    
    // Conversões implícitas para facilitar o uso
    public static implicit operator double(Weight weight) => weight.Value;
    public static implicit operator Weight(double value) => new(value);
}
```

**Contratos Implementados:**

1. **Validação no Construtor**: Impede criação de objetos inválidos
2. **Imutabilidade**: Value Objects são `record` imutáveis
3. **Exceções Específicas**: Cada violação gera exceção descritiva
4. **Fail-Fast**: Erros são detectados na criação, não no uso

### Entidade Delivery Imutável

```csharp
public class Delivery
{
    // Propriedades readonly garantem imutabilidade
    public Recipient Recipient { get; }
    public Address DeliveryAddress { get; }
    public Weight PackageWeight { get; }
    public ShippingType ShippingType { get; }
    public decimal ShippingCost { get; }
    public bool IsFreeShipping { get; }

    public Delivery(/* parâmetros */)
    {
        // Validações usando ArgumentNullException.ThrowIfNull
        Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
        // ...inicialização com cálculos
    }
}
```

**Robustez Garantida:**
- Estados inválidos são impossíveis de criar
- Exceções informativas facilitam debugging
- Imutabilidade previne corrupção de dados
- Validações centralizadas reduzem duplicação

## 3. Nomenclatura e Estrutura de Código

### Convenções de Nomenclatura Adotadas

**Classes e Interfaces:**
- `Delivery` (substituiu `Pedido`) - Nome em inglês, mais descritivo
- `IShippingCalculator` - Interface com prefixo 'I', nome autoexplicativo
- `EconomyShippingCalculator` - Nome específico substituindo códigos crípticos

**Enums e Extensões:**
```csharp
public enum ShippingType
{
    Express,    // Anteriormente "EXP"
    Standard,   // Anteriormente "PAD"  
    Economy     // Anteriormente "ECO"
}

public static class ShippingTypeExtensions
{
    public static string ToDisplayName(this ShippingType shippingType)
    {
        return shippingType switch
        {
            ShippingType.Express => "Expresso",
            ShippingType.Standard => "Padrão", 
            ShippingType.Economy => "Econômico",
            _ => shippingType.ToString()
        };
    }
}
```

**Métodos e Propriedades:**
- `CalculateShippingCost` (substituiu `calcularFrete`) - Verbo + substantivo claro
- `IsEligibleForFreeShipping` - Boolean com prefixo 'Is', semântica clara
- `GenerateShippingLabel` - Ação específica e propósito evidente

### Organização de Arquivos e Namespaces seguindo Clean Architecture

```
OrderManager.API/
├── Domain/                    # Camada de Domínio - Regras de Negócio
│   ├── Entities/              # Entidades de negócio
│   ├── ValueObjects/          # Objetos de valor (sem lógica)
│   ├── Interfaces/            # Contratos do domínio
│   ├── Services/              # Serviços de domínio
│   ├── Exceptions/            # Exceções específicas
│   └── Enums/                 # Enumerações
├── Application/               # Camada de Aplicação - Casos de Uso
│   └── Services/              # Serviços de aplicação
│       ├── Interfaces/        # Interfaces dos serviços
│       ├── Factories/         # Factories
│       └── Validation/        # Validações
├── Presentation/              # Camada de Apresentação - Interface Externa
│   ├── Controllers/           # Controladores da API
│   └── Contracts/             # DTOs (Requests/Responses)
├── Infrastructure/            # Camada de Infraestrutura - Detalhes Técnicos
│   ├── Configuration/         # Configurações de DI
│   └── CrossCutting/         # Utilitários transversais
└── Program.cs                # Configuração da aplicação
```

**Benefícios da Organização:**
- **Coesão Alta**: Arquivos relacionados agrupados
- **Baixo Acoplamento**: Separação clara de responsabilidades
- **Navegabilidade**: Estrutura intuitiva para desenvolvedores
- **Autodocumentação**: Código que se explica pelos nomes

## 4. Tratamento de Erros e Situações Anômalas

### Estratégia de Exceções Personalizadas

Implementamos uma hierarquia de exceções específicas do domínio:

```csharp
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}

public class InvalidWeightException : DomainException
{
    public InvalidWeightException(double weight) 
        : base($"Peso inválido: {weight}. O peso deve ser maior que zero.") { }
}
```

### Tratamento na Camada de Apresentação

```csharp
[HttpPost("create")]
public IActionResult CreateDelivery([FromBody] CreateDeliveryRequest request)
{
    try
    {
        // Lógica de criação...
        return Ok(response);
    }
    catch (DomainException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { error = "Erro interno do servidor" });
    }
}
```

**Abordagem Escolhida - Exceções:**

**Justificativa:**
- **Clareza**: Cada tipo de erro tem exceção específica
- **Rastreabilidade**: Stack trace facilita debugging
- **Fail-Fast**: Falhas são detectadas imediatamente
- **Separação**: Lógica de negócio separada do tratamento de erro

**Benefícios:**
- Mensagens de erro específicas e úteis
- Diferentes níveis de tratamento (domínio vs. infraestrutura)
- Previsibilidade para quem consome a API
- Facilita logging e monitoramento

## 5. Extensibilidade e Princípio Aberto-Fechado

### Arquitetura Preparada para Extensão

A arquitetura implementada permite extensões sem modificar código existente:

**Novos Tipos de Frete:**
```csharp
// Apenas implementar a interface
public class PremiumShippingCalculator : IShippingCalculator
{
    public decimal CalculateShippingCost(Weight weight)
    {
        return (decimal)weight.Value * 2.0m + 15m; // Lógica premium
    }
    
    public bool IsEligibleForFreeShipping(Weight weight) => false;
    public string GetShippingTypeName() => "Premium";
}

// Adicionar no enum
public enum ShippingType
{
    // ...existentes
    Premium     // Novo tipo
}

// Atualizar factory
public IShippingCalculator CreateCalculator(ShippingType shippingType)
{
    return shippingType switch
    {
        // ...cases existentes
        ShippingType.Premium => new PremiumShippingCalculator(),
        _ => throw new UnsupportedShippingTypeException(shippingType.ToString())
    };
}
```

**Novas Estratégias de Desconto:**
```csharp
public interface IDiscountStrategy
{
    Delivery ApplyDiscount(Delivery delivery);
}

public class VolumeDiscountStrategy : IDiscountStrategy
{
    public Delivery ApplyDiscount(Delivery delivery)
    {
        // Lógica de desconto por volume
    }
}
```

**Novos Formatos de Etiqueta:**
```csharp
public interface ILabelFormatter
{
    string Format(Delivery delivery);
}

public class QrCodeLabelFormatter : ILabelFormatter
{
    public string Format(Delivery delivery)
    {
        // Gerar etiqueta com QR Code
    }
}
```

### Eliminação de Condicionais Rígidas

**Antes (Código Legado):**
```java
public double calcularFrete() {
    if (tipoFrete.equals("EXP")) {
        return peso * 1.5 + 10;
    } else if (tipoFrete.equals("PAD")) {
        return peso * 1.2;
    } else if (tipoFrete.equals("ECO")) {
        return peso * 1.1 - 5;
    } else {
        return 0;
    }
}
```

**Depois (Polimorfismo):**
```csharp
// Sem condicionais - delegação para estratégia adequada
public decimal ShippingCost { get; }

private void CalculateShippingCost()
{
    var calculator = _calculatorFactory.CreateCalculator(ShippingType);
    ShippingCost = calculator.CalculateShippingCost(PackageWeight);
}
```

### Benefícios da Arquitetura Extensível

1. **Princípio Aberto-Fechado**: Aberto para extensão, fechado para modificação
2. **Composição sobre Herança**: Favorece flexibilidade
3. **Inversão de Dependência**: Depende de abstrações, não implementações
4. **Single Responsibility**: Cada classe tem uma responsabilidade única
5. **Facilita Testes**: Mocking de interfaces para testes isolados

### Exemplo de Extensão Futura

Para adicionar um sistema de promoções, bastaria:

```csharp
public interface IPromotionService
{
    decimal ApplyPromotions(Delivery delivery, IEnumerable<IPromotion> promotions);
}

public interface IPromotion
{
    bool IsApplicable(Delivery delivery);
    decimal CalculateDiscount(Delivery delivery);
    string GetPromotionName();
}

// Implementações específicas
public class BlackFridayPromotion : IPromotion { /* ... */ }
public class LoyaltyPromotion : IPromotion { /* ... */ }
```

Essa abordagem garante que o sistema possa evoluir sem quebrar funcionalidades existentes, mantendo a qualidade e estabilidade do código.

## Conclusão

A refatoração transformou um código legado problemático em uma arquitetura robusta, extensível e testável. Os princípios de Clean Code foram aplicados consistentemente, resultando em um sistema que não apenas resolve os problemas atuais, mas está preparado para futuras evoluções do negócio.
