using OrderManager.API.Application.Services.Interfaces.Validation;
using OrderManager.API.Domain.Enums;
using OrderManager.API.Domain.Exceptions;

namespace OrderManager.API.Application.Services.Validation
{
    public class ValidationService : IValidationService
    {
        public void ValidateWeight(double weight)
        {
            if (weight <= 0 || weight is double.NaN || weight is double.PositiveInfinity || weight is double.NegativeInfinity)
                throw new InvalidWeightException(weight);
        }

        public void ValidateAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new InvalidAddressException(address ?? "null");
        }

        public void ValidateRecipient(string recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient))
                throw new InvalidRecipientException(recipient ?? "null");
        }

        public void ValidateShippingType(string shippingType)
        {
            if (!Enum.TryParse<ShippingType>(shippingType, out var _))
                throw new InvalidShippingTypeException(shippingType ?? "null");
        }
    }
}
