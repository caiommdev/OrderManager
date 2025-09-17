namespace WebApplication1.Contracts.Requests
{
    public class CreateDeliveryRequest
    {
        public string Recipient { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Weight { get; set; }
        public string ShippingType { get; set; } = string.Empty;
    }
}
