using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Services.Factories;
using WebApplication1.Domain.ValueObjects;
using Xunit;

namespace OrderManager.Tests.Domain.Entities
{
    public class DeliveryTests
    {
        private readonly ShippingCalculatorFactory _calculatorFactory = new();

        [Fact]
        public void Constructor_ValidInputs_ShouldCreateDelivery()
        {
            // Arrange
            var recipient = new Recipient("João Silva");
            var address = new Address("Rua das Flores, 123");
            var weight = new Weight(5.0);
            var shippingType = ShippingType.Standard;

            // Act
            var delivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);

            // Assert
            Assert.Equal(recipient, delivery.Recipient);
            Assert.Equal(address, delivery.DeliveryAddress);
            Assert.Equal(weight, delivery.PackageWeight);
            Assert.Equal(shippingType, delivery.ShippingType);
            Assert.True(delivery.ShippingCost > 0);
        }

        [Fact]
        public void Constructor_EconomyShippingWithLightWeight_ShouldHaveFreeShipping()
        {
            // Arrange
            var recipient = new Recipient("Maria Santos");
            var address = new Address("Av. Principal, 456");
            var weight = new Weight(1.5); // Abaixo do limite de 2kg
            var shippingType = ShippingType.Economy;

            // Act
            var delivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);

            // Assert
            Assert.True(delivery.IsFreeShipping);
            Assert.Equal(0m, delivery.ShippingCost);
        }

        [Fact]
        public void Constructor_ExpressShipping_ShouldNeverHaveFreeShipping()
        {
            // Arrange
            var recipient = new Recipient("Carlos Oliveira");
            var address = new Address("Rua Pequena, 789");
            var weight = new Weight(0.5); // Peso muito baixo
            var shippingType = ShippingType.Express;

            // Act
            var delivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);

            // Assert
            Assert.False(delivery.IsFreeShipping);
            Assert.True(delivery.ShippingCost > 0);
        }

        [Fact]
        public void ApplyPromotionalDiscount_WeightAbove10_ShouldReduceWeightAndRecalculate()
        {
            // Arrange
            var recipient = new Recipient("Ana Costa");
            var address = new Address("Rua Grande, 101");
            var weight = new Weight(15.0);
            var shippingType = ShippingType.Standard;

            var originalDelivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);
            var originalCost = originalDelivery.ShippingCost;

            // Act
            var discountedDelivery = originalDelivery.ApplyPromotionalDiscount();

            // Assert
            Assert.Equal(14.0, discountedDelivery.PackageWeight.Value); // Redução de 1kg
            Assert.True(discountedDelivery.ShippingCost < originalCost); // Custo menor
        }

        [Fact]
        public void ApplyPromotionalDiscount_WeightBelow10_ShouldNotChangeDelivery()
        {
            // Arrange
            var recipient = new Recipient("Pedro Lima");
            var address = new Address("Rua Média, 202");
            var weight = new Weight(8.0);
            var shippingType = ShippingType.Standard;

            var originalDelivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);

            // Act
            var discountedDelivery = originalDelivery.ApplyPromotionalDiscount();

            // Assert
            Assert.Equal(originalDelivery.PackageWeight.Value, discountedDelivery.PackageWeight.Value);
            Assert.Equal(originalDelivery.ShippingCost, discountedDelivery.ShippingCost);
        }

        [Fact]
        public void Constructor_NullRecipient_ShouldThrowArgumentNullException()
        {
            // Arrange
            var address = new Address("Rua das Flores, 123");
            var weight = new Weight(5.0);
            var shippingType = ShippingType.Standard;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new Delivery(null!, address, weight, shippingType, _calculatorFactory));
        }

        [Fact]
        public void ToString_ShouldReturnFormattedString()
        {
            // Arrange
            var recipient = new Recipient("Roberto Silva");
            var address = new Address("Av. Central, 300");
            var weight = new Weight(3.0);
            var shippingType = ShippingType.Express;

            var delivery = new Delivery(recipient, address, weight, shippingType, _calculatorFactory);

            // Act
            var result = delivery.ToString();

            // Assert
            Assert.Contains("Roberto Silva", result);
            Assert.Contains("Expresso", result);
            Assert.Contains("R$", result);
        }
    }
}
