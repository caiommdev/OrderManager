namespace WebApplication1.Contracts.Responses
{
    public class PromotionalDiscountResponse
    {
        public decimal OriginalCost { get; set; }
        public decimal DiscountedCost { get; set; }
        public decimal Savings { get; set; }
        public bool DiscountApplied { get; set; }
        public DeliveryResponse FinalDelivery { get; set; } = new();
    }
}
