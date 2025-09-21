using OrderManager.API.Domain.Entities;

namespace OrderManager.API.Application.Services.Interfaces
{
    public interface ILabelService
    {
        string GenerateShippingLabel(Order delivery);
        string GenerateOrderSummary(Order delivery);
    }
}
