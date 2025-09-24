using FluentAssertions;
using Moq;
using OrderManager.API.Application.Services;
using OrderManager.API.Domain.Entities;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.ValueObjects;
using OrderManager.API.Domain.Services.Factories;
using Xunit;

namespace OrderManager.Tests.Application.Services
{
    public class LabelServiceTests
    {
        private readonly LabelService _labelService;
        private readonly Mock<IShippingCalculatorFactory> _mockCalculatorFactory;

        public LabelServiceTests()
        {
            _labelService = new LabelService();
            _mockCalculatorFactory = new Mock<IShippingCalculatorFactory>();
        }

        [Fact]
        public void GenerateShippingLabel_WithValidOrder_ShouldGenerateCorrectLabel()
        {
            // Arrange
            var recipient = new Recipient("João Silva");
            var address = new Address("Rua das Flores, 123");
            var weight = new Weight(2.5);
            var shippingType = ShippingType.Standard;
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateShippingLabel(order);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("ETIQUETA DE ENTREGA");
            result.Should().Contain("João Silva");
            result.Should().Contain("Rua das Flores, 123");
            result.Should().Contain("2,5");
            result.Should().Contain("Padrão");
            result.Should().Contain("R$");
            result.Should().Contain("╔");
            result.Should().Contain("╚");
        }

        [Fact]
        public void GenerateShippingLabel_WithFreeShippingOrder_ShouldIncludeFreeShippingMessage()
        {
            // Arrange
            var recipient = new Recipient("Maria Santos");
            var address = new Address("Av. Paulista, 1000");
            var weight = new Weight(1.0);
            var shippingType = ShippingType.Economy;
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateShippingLabel(order);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("🎉 FRETE GRÁTIS! 🎉");
            result.Should().Contain("Maria Santos");
            result.Should().Contain("Av. Paulista, 1000");
            result.Should().Contain("Econômico");
        }

        [Fact]
        public void GenerateShippingLabel_WithNullOrder_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => _labelService.GenerateShippingLabel(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GenerateOrderSummary_WithValidOrder_ShouldGenerateCorrectSummary()
        {
            // Arrange
            var recipient = new Recipient("Carlos Pereira");
            var address = new Address("Rua das Palmeiras, 456");
            var weight = new Weight(3.0);
            var shippingType = ShippingType.Express;
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateOrderSummary(order);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Carlos Pereira");
            result.Should().Contain("Expresso");
            result.Should().Contain("R$");
            result.Should().StartWith("Pedido para");
        }

        [Fact]
        public void GenerateOrderSummary_WithFreeShippingOrder_ShouldIncludeFreeShippingIndicator()
        {
            // Arrange
            var recipient = new Recipient("Ana Costa");
            var address = new Address("Rua das Acácias, 789");
            var weight = new Weight(1.5);
            var shippingType = ShippingType.Economy;
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateOrderSummary(order);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Ana Costa");
            result.Should().Contain("Econômico");
            result.Should().Contain("(FRETE GRÁTIS)");
            result.Should().EndWith("(FRETE GRÁTIS)");
        }

        [Fact]
        public void GenerateOrderSummary_WithNullOrder_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => _labelService.GenerateOrderSummary(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(ShippingType.Standard, "Padrão")]
        [InlineData(ShippingType.Express, "Expresso")]
        [InlineData(ShippingType.Economy, "Econômico")]
        public void GenerateShippingLabel_WithDifferentShippingTypes_ShouldDisplayCorrectTypeName(ShippingType shippingType, string expectedTypeName)
        {
            // Arrange
            var recipient = new Recipient("Teste Cliente");
            var address = new Address("Endereço Teste, 123");
            var weight = new Weight(5.0);
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateShippingLabel(order);

            // Assert
            result.Should().Contain(expectedTypeName);
        }

        [Theory]
        [InlineData(ShippingType.Standard, "Padrão")]
        [InlineData(ShippingType.Express, "Expresso")]
        [InlineData(ShippingType.Economy, "Econômico")]
        public void GenerateOrderSummary_WithDifferentShippingTypes_ShouldDisplayCorrectTypeName(ShippingType shippingType, string expectedTypeName)
        {
            // Arrange
            var recipient = new Recipient("Teste Cliente");
            var address = new Address("Endereço Teste, 123");
            var weight = new Weight(5.0);
            
            var order = new Order(recipient, address, weight, shippingType, _mockCalculatorFactory.Object);

            // Act
            var result = _labelService.GenerateOrderSummary(order);

            // Assert
            result.Should().Contain(expectedTypeName);
        }
    }
}