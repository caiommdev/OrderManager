using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.ValueObjects;

namespace OrderManager.API.Application.Services.ShippingCalculators
{
    public class ExpressShippingCalculator : IShippingCalculator
    {
        private const decimal WeightMultiplier = 1.5m;
        private const decimal FixedFee = 10m;

        public decimal CalculateShippingCost(Weight weight)
        {
            return (decimal)weight.Value * WeightMultiplier + FixedFee;
        }

        public bool IsEligibleForFreeShipping(Weight weight)
        {
            return false;
        }

        public string GetShippingTypeName()
        {
            return "Expresso";
        }
    }
}
