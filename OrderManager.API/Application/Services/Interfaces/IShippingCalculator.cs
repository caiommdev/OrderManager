using OrderManager.API.Domain.ValueObjects;

namespace OrderManager.API.Application.Services.Interfaces
{
    public interface IShippingCalculator
    {
        decimal CalculateShippingCost(Weight weight);
        bool IsEligibleForFreeShipping(Weight weight);
        string GetShippingTypeName();
    }
}
