using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Domain.Entities;
using OrderManager.API.Domain.Enums;

namespace OrderManager.API.Application.Services
{
    public class LabelService : ILabelService
    {
        public string GenerateShippingLabel(Order delivery)
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

        public string GenerateOrderSummary(Order delivery)
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
