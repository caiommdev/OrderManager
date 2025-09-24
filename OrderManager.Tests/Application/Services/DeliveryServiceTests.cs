using FluentAssertions;
using Moq;
using OrderManager.API.Application.Services;
using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.Entities;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.Exceptions;
using OrderManager.API.Domain.Services.Factories;
using Xunit;

namespace OrderManager.Tests.Application.Services
{
    public class DeliveryServiceTests
    {
        private readonly DeliveryService _deliveryService;
        private readonly Mock<IShippingCalculatorFactory> _mockCalculatorFactory;
        private readonly Mock<IShippingCalculator> _mockCalculator;

        public DeliveryServiceTests()
        {
            _mockCalculatorFactory = new Mock<IShippingCalculatorFactory>();
            _mockCalculator = new Mock<IShippingCalculator>();
            _deliveryService = new DeliveryService(_mockCalculatorFactory.Object);
        }

        [Theory]
        [InlineData("João Silva", "Rua das Flores, 123", 2.5, "PAD")]
        [InlineData("Maria Santos", "Av. Paulista, 1000", 1.0, "ECO")]
        [InlineData("Carlos Pereira", "Rua das Palmeiras, 456", 3.0, "EXP")]
        public void CreateDelivery_WithValidParameters_ShouldCreateOrderSuccessfully(
            string recipient, string address, double weight, string shippingTypeCode)
        {
            // Arrange
            _mockCalculatorFactory.Setup(x => x.CreateCalculator(It.IsAny<ShippingType>()))
                                 .Returns(_mockCalculator.Object);
            _mockCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(10.00m);
            _mockCalculator.Setup(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(false);

            // Act
            var result = _deliveryService.CreateDelivery(recipient, address, weight, shippingTypeCode);

            // Assert
            result.Should().NotBeNull();
            result.Recipient.Name.Should().Be(recipient);
            result.DeliveryAddress.Value.Should().Be(address);
            result.PackageWeight.Value.Should().Be(weight);
            result.ShippingCost.Should().Be(10.00m);
        }

        // Note: As validações estão no ValidationService, não são chamadas automaticamente pelo DeliveryService
        // Os testes de validação devem estar no ValidationServiceTests

        [Theory]
        [InlineData("João Silva", "Rua das Flores, 123", 2.5, "INVALID")]
        [InlineData("João Silva", "Rua das Flores, 123", 2.5, "XYZ")]
        [InlineData("João Silva", "Rua das Flores, 123", 2.5, "")]
        [InlineData("João Silva", "Rua das Flores, 123", 2.5, null)]
        public void CreateDelivery_WithInvalidShippingType_ShouldThrowArgumentException(
            string recipient, string address, double weight, string shippingTypeCode)
        {
            // Act & Assert
            var act = () => _deliveryService.CreateDelivery(recipient, address, weight, shippingTypeCode);
            act.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData("PAD", ShippingType.Standard)]
        [InlineData("EXP", ShippingType.Express)]
        [InlineData("ECO", ShippingType.Economy)]
        public void CreateDelivery_WithValidShippingTypeCodes_ShouldMapToCorrectEnum(
            string shippingTypeCode, ShippingType expectedShippingType)
        {
            // Arrange
            _mockCalculatorFactory.Setup(x => x.CreateCalculator(It.IsAny<ShippingType>()))
                                 .Returns(_mockCalculator.Object);
            _mockCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(10.00m);
            _mockCalculator.Setup(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(false);

            // Act
            var result = _deliveryService.CreateDelivery("João Silva", "Rua das Flores, 123", 2.5, shippingTypeCode);

            // Assert
            result.ShippingType.Should().Be(expectedShippingType);
        }

        [Fact]
        public void ApplyPromotionalDiscount_WithWeightAbove10kg_ShouldReduceWeightBy1kg()
        {
            // Arrange
            _mockCalculatorFactory.Setup(x => x.CreateCalculator(It.IsAny<ShippingType>()))
                                 .Returns(_mockCalculator.Object);
            _mockCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(15.00m);
            _mockCalculator.Setup(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(false);

            var originalOrder = _deliveryService.CreateDelivery("João Silva", "Rua das Flores, 123", 12.0, "PAD");

            // Act
            var discountedOrder = _deliveryService.ApplyPromotionalDiscount(originalOrder);

            // Assert
            discountedOrder.Should().NotBeNull();
            discountedOrder.PackageWeight.Value.Should().Be(11.0);
            discountedOrder.Recipient.Name.Should().Be(originalOrder.Recipient.Name);
            discountedOrder.DeliveryAddress.Value.Should().Be(originalOrder.DeliveryAddress.Value);
            discountedOrder.ShippingType.Should().Be(originalOrder.ShippingType);
        }

        [Theory]
        [InlineData(5.0)]
        [InlineData(10.0)]
        [InlineData(8.5)]
        [InlineData(1.0)]
        public void ApplyPromotionalDiscount_WithWeightUpTo10kg_ShouldReturnSameOrder(double weight)
        {
            // Arrange
            _mockCalculatorFactory.Setup(x => x.CreateCalculator(It.IsAny<ShippingType>()))
                                 .Returns(_mockCalculator.Object);
            _mockCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(10.00m);
            _mockCalculator.Setup(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(false);

            var originalOrder = _deliveryService.CreateDelivery("João Silva", "Rua das Flores, 123", weight, "PAD");

            // Act
            var discountedOrder = _deliveryService.ApplyPromotionalDiscount(originalOrder);

            // Assert
            discountedOrder.Should().BeSameAs(originalOrder);
            discountedOrder.PackageWeight.Value.Should().Be(weight);
        }

        [Fact]
        public void CreateDelivery_ShouldUseCalculatorToCalculateShippingCost()
        {
            // Arrange
            var expectedCost = 25.50m;
            _mockCalculatorFactory.Setup(x => x.CreateCalculator(It.IsAny<ShippingType>()))
                                 .Returns(_mockCalculator.Object);
            _mockCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(expectedCost);
            _mockCalculator.Setup(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()))
                          .Returns(false);

            // Act
            var result = _deliveryService.CreateDelivery("João Silva", "Rua das Flores, 123", 2.5, "PAD");

            // Assert
            result.ShippingCost.Should().Be(expectedCost);
            _mockCalculator.Verify(x => x.CalculateShippingCost(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()), Times.Once);
            _mockCalculator.Verify(x => x.IsEligibleForFreeShipping(It.IsAny<OrderManager.API.Domain.ValueObjects.Weight>()), Times.Once);
        }
    }
}