using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Interfaces;

namespace WebApplication1.Domain.Services.Factories
{
    public interface IShippingCalculatorFactory
    {
        IShippingCalculator CreateCalculator(ShippingType shippingType);
    }
}
