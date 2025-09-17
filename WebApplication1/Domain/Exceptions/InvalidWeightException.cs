namespace WebApplication1.Domain.Exceptions
{
    public class InvalidWeightException : DomainException
    {
        public InvalidWeightException(double weight) 
            : base($"Peso inválido: {weight}. O peso deve ser maior que zero.")
        {
        }
    }
}
