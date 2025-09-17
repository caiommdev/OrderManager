using WebApplication1.Domain.Exceptions;

namespace WebApplication1.Domain.Services.Validation
{
    public class ValidationService : IValidationService
    {
        public void ValidateWeight(double weight)
        {
            if (weight <= 0)
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
    }
}
