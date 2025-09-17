namespace WebApplication1.Contracts.Responses
{
    public class DeliveryResponse
    {
        public string Recipient { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string ShippingType { get; set; } = string.Empty;
        public decimal ShippingCost { get; set; }
        public bool IsFreeShipping { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
    }
}
