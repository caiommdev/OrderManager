using OrderManager.API.Domain.Entities;

namespace OrderManager.API.Application.Services.Interfaces
{
    public interface IDeliveryService
    {
        Order CreateDelivery(string recipient, string address, double weight, string shippingTypeCode);
        Order ApplyPromotionalDiscount(Order delivery);
    }
}
