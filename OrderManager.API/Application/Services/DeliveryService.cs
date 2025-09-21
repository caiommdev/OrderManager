using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Application.Services.Interfaces.Factories;
using OrderManager.API.Application.Services.Interfaces.Validation;
using OrderManager.API.Domain.Entities;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.ValueObjects;

namespace OrderManager.API.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IValidationService _validationService;
        private readonly IShippingCalculatorFactory _calculatorFactory;

        public DeliveryService(IValidationService validationService, IShippingCalculatorFactory calculatorFactory)
        {
            _validationService = validationService;
            _calculatorFactory = calculatorFactory;
        }

        public Order CreateDelivery(string recipient, string address, double weight, string shippingTypeCode)
        {
            _validationService.ValidateRecipient(recipient);
            _validationService.ValidateAddress(address);
            _validationService.ValidateWeight(weight);

            var recipientVO = new Recipient(recipient);
            var addressVO = new Address(address);
            var weightVO = new Weight(weight);
            var shippingType = ShippingTypeExtensions.FromCode(shippingTypeCode);

            return new Order(recipientVO, addressVO, weightVO, shippingType, _calculatorFactory);
        }

        public Order ApplyPromotionalDiscount(Order order)
        {
            return order.ApplyPromotionalDiscount();
        }
    }
}
