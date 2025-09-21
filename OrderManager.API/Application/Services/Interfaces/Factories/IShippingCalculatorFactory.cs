using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.Enums;

namespace OrderManager.API.Domain.Services.Factories
{
    public interface IShippingCalculatorFactory
    {
        IShippingCalculator CreateCalculator(ShippingType shippingType);
    }
}
