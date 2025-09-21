using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Application.Services.Interfaces.Factories;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Application.Services.Factories;
using OrderManager.API.Domain.ValueObjects;

namespace OrderManager.API.Domain.Entities
{
    public class Order
    {
        public Recipient Recipient { get; }
        public Address DeliveryAddress { get; }
        public Weight PackageWeight { get; }
        public ShippingType ShippingType { get; }
        public decimal ShippingCost { get; }
        public bool IsFreeShipping { get; }

        private readonly IShippingCalculator _shippingCalculator;

        public Order(
            Recipient recipient,
            Address deliveryAddress,
            Weight packageWeight,
            ShippingType shippingType,
            IShippingCalculatorFactory? calculatorFactory = null)
        {
            Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PackageWeight = packageWeight ?? throw new ArgumentNullException(nameof(packageWeight));
            ShippingType = shippingType;

            _shippingCalculator = calculatorFactory?.CreateCalculator(shippingType) ?? new ShippingCalculatorFactory().CreateCalculator(shippingType);
            ShippingCost = _shippingCalculator.CalculateShippingCost(packageWeight);
            IsFreeShipping = _shippingCalculator.IsEligibleForFreeShipping(packageWeight);
        }

        public Order ApplyPromotionalDiscount()
        {
            if (PackageWeight.Value <= 10)
                return this;

            var discountedWeight = new Weight(PackageWeight.Value - 1);
            return new Order(Recipient, DeliveryAddress, discountedWeight, ShippingType);
        }

        public override string ToString()
        {
            return $"Entrega para {Recipient} - {ShippingType.ToDisplayName()} - R$ {ShippingCost:F2}";
        }
    }
}
