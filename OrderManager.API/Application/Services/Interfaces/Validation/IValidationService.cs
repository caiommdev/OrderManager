namespace OrderManager.API.Application.Services.Interfaces.Validation
{
    public interface IValidationService
    {
        void ValidateWeight(double weight);
        void ValidateAddress(string address);
        void ValidateRecipient(string recipient);
        void ValidateShippingType(string shippingType);
    }
}
