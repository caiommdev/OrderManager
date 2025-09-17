using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Enums;

namespace WebApplication1.Domain.Services
{
    public class LabelService : ILabelService
    {
        public string GenerateShippingLabel(Delivery delivery)
        {
            ArgumentNullException.ThrowIfNull(delivery);

            var label = $"""
                ╔══════════════════════════════════════╗
                ║              ETIQUETA DE ENTREGA     ║
                ╠══════════════════════════════════════╣
                ║ Destinatário: {delivery.Recipient.Name,-20} ║
                ║ Endereço: {delivery.DeliveryAddress.Value,-24} ║
                ║ Peso: {delivery.PackageWeight,-28} ║
                ║ Tipo de Frete: {delivery.ShippingType.ToDisplayName(),-19} ║
                ║ Valor do Frete: R$ {delivery.ShippingCost:F2,-17} ║
                {(delivery.IsFreeShipping ? "║ 🎉 FRETE GRÁTIS! 🎉                 ║" : string.Empty)}
                ╚══════════════════════════════════════╝
                """;

            return label;
        }

        public string GenerateOrderSummary(Delivery delivery)
        {
            ArgumentNullException.ThrowIfNull(delivery);

            var summary = $"Pedido para {delivery.Recipient.Name} " +
                         $"com frete tipo {delivery.ShippingType.ToDisplayName()} " +
                         $"no valor de R$ {delivery.ShippingCost:F2}";

            if (delivery.IsFreeShipping)
            {
                summary += " (FRETE GRÁTIS)";
            }

            return summary;
        }
    }
}
