using Moq;
using WebApplication1.Application.Services;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Exceptions;
using WebApplication1.Domain.Services.Factories;
using WebApplication1.Domain.Services.ShippingCalculators;
using WebApplication1.Domain.Services.Validation;
using Xunit;

namespace OrderManager.Tests.Application.Services
{
    public class DeliveryServiceTests
    {
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly Mock<IShippingCalculatorFactory> _mockCalculatorFactory;
        private readonly DeliveryService _deliveryService;

        public DeliveryServiceTests()
        {
            _mockValidationService = new Mock<IValidationService>();
            _mockCalculatorFactory = new Mock<IShippingCalculatorFactory>();
            _deliveryService = new DeliveryService(_mockValidationService.Object, _mockCalculatorFactory.Object);
        }

        [Fact]
        public void CreateDelivery_ValidInputs_ShouldCreateDelivery()
        {
            var recipient = "João Silva";
            var address = "Rua das Flores, 123";
            var weight = 5.0;
            var shippingTypeCode = "PAD";

            _mockCalculatorFactory
                .Setup(x => x.CreateCalculator(ShippingType.Standard))
                .Returns(new StandardShippingCalculator());

            var delivery = _deliveryService.CreateDelivery(recipient, address, weight, shippingTypeCode);

            Assert.Equal(recipient, delivery.Recipient.Name);
            Assert.Equal(address, delivery.DeliveryAddress.Value);
            Assert.Equal(weight, delivery.PackageWeight.Value);
            Assert.Equal(ShippingType.Standard, delivery.ShippingType);

            _mockValidationService.Verify(x => x.ValidateRecipient(recipient), Times.Once);
            _mockValidationService.Verify(x => x.ValidateAddress(address), Times.Once);
            _mockValidationService.Verify(x => x.ValidateWeight(weight), Times.Once);
        }

        [Fact]
        public void CreateDelivery_InvalidWeight_ShouldThrowException()
        {
            var recipient = "João Silva";
            var address = "Rua das Flores, 123";
            var weight = -5.0;
            var shippingTypeCode = "PAD";

            _mockValidationService
                .Setup(x => x.ValidateWeight(weight))
                .Throws(new InvalidWeightException(weight));

            Assert.Throws<InvalidWeightException>(() => 
                _deliveryService.CreateDelivery(recipient, address, weight, shippingTypeCode));
        }

        [Fact]
        public void ApplyPromotionalDiscount_ValidDelivery_ShouldReturnDiscountedDelivery()
        {
            var recipient = "Ana Costa";
            var address = "Rua Grande, 101";
            var weight = 15.0;
            var shippingTypeCode = "PAD";

            _mockCalculatorFactory
                .Setup(x => x.CreateCalculator(ShippingType.Standard))
                .Returns(new StandardShippingCalculator());

            var originalDelivery = _deliveryService.CreateDelivery(recipient, address, weight, shippingTypeCode);
            var discountedDelivery = _deliveryService.ApplyPromotionalDiscount(originalDelivery);

            Assert.Equal(14.0, discountedDelivery.PackageWeight.Value);
            Assert.True(discountedDelivery.ShippingCost < originalDelivery.ShippingCost);
        }
    }
}
