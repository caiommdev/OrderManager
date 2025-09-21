using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.Enums;

namespace OrderManager.API.Application.Services.Interfaces.Factories
{
    public interface IShippingCalculatorFactory
    {
        IShippingCalculator CreateCalculator(ShippingType shippingType);
    }
}
