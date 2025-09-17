namespace WebApplication1.Domain.ValueObjects
{
    public record Weight
    {
        public double Value { get; }

        public Weight(double value)
        {
            Value = value;
        }

        public static implicit operator double(Weight weight) => weight.Value;
        public static implicit operator Weight(double value) => new(value);

        public override string ToString() => $"{Value:F2} kg";
    }
}
