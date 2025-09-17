using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Services.Factories;
using WebApplication1.Domain.Services.Validation;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Services
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

        public Delivery CreateDelivery(string recipient, string address, double weight, string shippingTypeCode)
        {
            _validationService.ValidateRecipient(recipient);
            _validationService.ValidateAddress(address);
            _validationService.ValidateWeight(weight);

            var recipientVO = new Recipient(recipient);
            var addressVO = new Address(address);
            var weightVO = new Weight(weight);
            var shippingType = ShippingTypeExtensions.FromCode(shippingTypeCode);

            return new Delivery(recipientVO, addressVO, weightVO, shippingType, _calculatorFactory);
        }

        public Delivery ApplyPromotionalDiscount(Delivery delivery)
        {
            return delivery.ApplyPromotionalDiscount();
        }
    }
}
