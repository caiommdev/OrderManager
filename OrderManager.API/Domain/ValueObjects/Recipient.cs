namespace OrderManager.API.Domain.ValueObjects
{
    public record Recipient
    {
        public string Name { get; }

        public Recipient(string name)
        {
            Name = name?.Trim() ?? string.Empty;
        }

        public static implicit operator string(Recipient recipient) => recipient.Name;
        public static implicit operator Recipient(string name) => new(name);

        public override string ToString() => Name;
    }
}
