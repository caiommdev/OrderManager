namespace OrderManager.API.Domain.Exceptions
{
    public class UnsupportedShippingTypeException : DomainException
    {
        public UnsupportedShippingTypeException(string shippingType) 
            : base($"Tipo de frete não suportado: '{shippingType}'.")
        {
        }
    }
}
