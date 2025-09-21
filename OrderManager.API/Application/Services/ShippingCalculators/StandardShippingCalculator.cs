using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.ValueObjects;

namespace OrderManager.API.Application.Services.ShippingCalculators
{
    public class StandardShippingCalculator : IShippingCalculator
    {
        private const decimal WeightMultiplier = 1.2m;

        public decimal CalculateShippingCost(Weight weight)
        {
            return (decimal)weight.Value * WeightMultiplier;
        }

        public bool IsEligibleForFreeShipping(Weight weight)
        {
            return false;
        }

        public string GetShippingTypeName()
        {
            return "Padrão";
        }
    }
}
