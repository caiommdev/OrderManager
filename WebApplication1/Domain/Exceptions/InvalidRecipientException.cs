namespace WebApplication1.Domain.Exceptions
{
    public class InvalidRecipientException : DomainException
    {
        public InvalidRecipientException(string recipient) 
            : base($"Destinatário inválido: '{recipient}'. O destinatário não pode ser nulo ou vazio.")
        {
        }
    }
}
