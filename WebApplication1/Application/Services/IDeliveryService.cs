using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Enums;
using WebApplication1.Domain.Services.Factories;
using WebApplication1.Domain.Services.Validation;
using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Services
{
    public interface IDeliveryService
    {
        Delivery CreateDelivery(string recipient, string address, double weight, string shippingTypeCode);
        Delivery ApplyPromotionalDiscount(Delivery delivery);
    }
}
