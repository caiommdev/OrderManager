using OrderManager.API.Domain.Exceptions;

    internal class InvalidShippingTypeException : DomainException
    {
        public InvalidShippingTypeException(string? message) : base(message)
        {
        }

        public InvalidShippingTypeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }