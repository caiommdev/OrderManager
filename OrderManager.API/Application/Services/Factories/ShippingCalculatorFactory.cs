using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.Exceptions;
using OrderManager.API.Application.Services.ShippingCalculators;
using OrderManager.API.Domain.Services.Factories;

namespace OrderManager.API.Application.Services.Factories
{
    public class ShippingCalculatorFactory : IShippingCalculatorFactory
    {
        public IShippingCalculator CreateCalculator(ShippingType shippingType)
        {
            return shippingType switch
            {
                ShippingType.Express => new ExpressShippingCalculator(),
                ShippingType.Standard => new StandardShippingCalculator(),
                ShippingType.Economy => new EconomyShippingCalculator(),
                _ => throw new UnsupportedShippingTypeException(shippingType.ToString())
            };
        }
    }
}
