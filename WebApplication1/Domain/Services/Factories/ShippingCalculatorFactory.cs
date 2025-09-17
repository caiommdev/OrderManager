using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Exceptions;
using WebApplication1.Domain.Interfaces;
using WebApplication1.Domain.Services.ShippingCalculators;

namespace WebApplication1.Domain.Services.Factories
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
