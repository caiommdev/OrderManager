using WebApplication1.Domain.Entities;

namespace WebApplication1.Domain.Services
{
    public interface ILabelService
    {
        string GenerateShippingLabel(Delivery delivery);
        string GenerateOrderSummary(Delivery delivery);
    }
}
