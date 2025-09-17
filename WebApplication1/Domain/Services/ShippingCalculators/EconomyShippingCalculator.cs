using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Services.ShippingCalculators
{
    public class EconomyShippingCalculator : IShippingCalculator
    {
        private const decimal WeightMultiplier = 1.1m;
        private const decimal Discount = 5m;
        private const double FreeShippingWeightLimit = 2.0;

        public decimal CalculateShippingCost(Weight weight)
        {
            if (IsEligibleForFreeShipping(weight))
                return 0m;

            var cost = (decimal)weight.Value * WeightMultiplier - Discount;
            return Math.Max(cost, 0m);
        }

        public bool IsEligibleForFreeShipping(Weight weight)
        {
            return weight.Value < FreeShippingWeightLimit;
        }

        public string GetShippingTypeName()
        {
            return "Econômico";
        }
    }
}
