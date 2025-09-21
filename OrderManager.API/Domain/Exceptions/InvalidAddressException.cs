namespace OrderManager.API.Domain.Exceptions
{
    public class InvalidAddressException : DomainException
    {
        public InvalidAddressException(string address) 
            : base($"Endereço inválido: '{address}'. O endereço não pode ser nulo ou vazio.")
        {
        }
    }
}
