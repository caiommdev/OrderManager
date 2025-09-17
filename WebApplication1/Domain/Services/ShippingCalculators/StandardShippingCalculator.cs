using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Services.ShippingCalculators
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
