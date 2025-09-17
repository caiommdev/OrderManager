namespace WebApplication1.Domain.Enums
{
    public enum ShippingType
    {
        Express,
        Standard,
        Economy
    }

    public static class ShippingTypeExtensions
    {
        public static string ToDisplayName(this ShippingType shippingType)
        {
            return shippingType switch
            {
                ShippingType.Express => "Expresso",
                ShippingType.Standard => "Padrão",
                ShippingType.Economy => "Econômico",
                _ => shippingType.ToString()
            };
        }

        public static string ToCode(this ShippingType shippingType)
        {
            return shippingType switch
            {
                ShippingType.Express => "EXP",
                ShippingType.Standard => "PAD",
                ShippingType.Economy => "ECO",
                _ => shippingType.ToString()
            };
        }

        public static ShippingType FromCode(string code)
        {
            return code?.ToUpper() switch
            {
                "EXP" => ShippingType.Express,
                "PAD" => ShippingType.Standard,
                "ECO" => ShippingType.Economy,
                _ => throw new ArgumentException($"Código de frete inválido: {code}")
            };
        }
    }
}
