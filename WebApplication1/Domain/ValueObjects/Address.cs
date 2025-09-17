namespace WebApplication1.Domain.ValueObjects
{
    public record Address
    {
        public string Value { get; }

        public Address(string value)
        {
            Value = value?.Trim() ?? string.Empty;
        }

        public static implicit operator string(Address address) => address.Value;
        public static implicit operator Address(string value) => new(value);

        public override string ToString() => Value;
    }
}
