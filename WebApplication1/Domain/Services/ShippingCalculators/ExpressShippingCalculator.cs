using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Services.ShippingCalculators
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
