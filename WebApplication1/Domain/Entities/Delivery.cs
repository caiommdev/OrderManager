using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.Services.Factories;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Entities
{
    public class Delivery
    {
        public Recipient Recipient { get; }
        public Address DeliveryAddress { get; }
        public Weight PackageWeight { get; }
        public ShippingType ShippingType { get; }
        public decimal ShippingCost { get; }
        public bool IsFreeShipping { get; }

        private readonly IShippingCalculator _shippingCalculator;

        public Delivery(
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

        public Delivery ApplyPromotionalDiscount()
        {
            if (PackageWeight.Value <= 10)
                return this;

            var discountedWeight = new Weight(PackageWeight.Value - 1);
            return new Delivery(Recipient, DeliveryAddress, discountedWeight, ShippingType);
        }

        public override string ToString()
        {
            return $"Entrega para {Recipient} - {ShippingType.ToDisplayName()} - R$ {ShippingCost:F2}";
        }
    }
}
