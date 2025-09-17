using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Domain.Interfaces
{
    public interface IShippingCalculator
    {
        decimal CalculateShippingCost(Weight weight);
        bool IsEligibleForFreeShipping(Weight weight);
        string GetShippingTypeName();
    }
}
