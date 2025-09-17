using WebApplication1.Domain.ValueObjects;
using Xunit;

namespace OrderManager.Tests.Domain.ValueObjects
{
    public class WeightTests
    {
        [Fact]
        public void Constructor_ValidWeight_ShouldCreateWeight()
        {
            var value = 5.0;
            var weight = new Weight(value);
            Assert.Equal(value, weight.Value);
        }

        [Fact]
        public void Constructor_ZeroWeight_ShouldCreateWeight()
        {
            var value = 0.0;
            var weight = new Weight(value);
            Assert.Equal(value, weight.Value);
        }

        [Fact]
        public void Constructor_NegativeWeight_ShouldCreateWeight()
        {
            var value = -5.0;
            var weight = new Weight(value);
            Assert.Equal(value, weight.Value);
        }

        [Fact]
        public void ImplicitConversion_FromDouble_ShouldWork()
        {
            double value = 3.5;
            Weight weight = value;
            Assert.Equal(value, weight.Value);
        }

        [Fact]
        public void ImplicitConversion_ToDouble_ShouldWork()
        {
            var weight = new Weight(7.2);
            double value = weight;
            Assert.Equal(7.2, value);
        }

        [Fact]
        public void ToString_ShouldFormatCorrectly()
        {
            var weight = new Weight(5.25);
            var result = weight.ToString();
            Assert.Equal("5,25 kg", result);
        }
    }

    public class AddressTests
    {
        [Fact]
        public void Constructor_ValidAddress_ShouldCreateAddress()
        {
            var value = "Rua das Flores, 123";
            var address = new Address(value);
            Assert.Equal(value, address.Value);
        }

        [Fact]
        public void Constructor_NullAddress_ShouldCreateEmptyAddress()
        {
            var address = new Address(null);
            Assert.Equal(string.Empty, address.Value);
        }

        [Fact]
        public void Constructor_EmptyAddress_ShouldCreateEmptyAddress()
        {
            var address = new Address("");
            Assert.Equal(string.Empty, address.Value);
        }

        [Fact]
        public void Constructor_WhitespaceAddress_ShouldCreateEmptyAddress()
        {
            var address = new Address("   ");
            Assert.Equal(string.Empty, address.Value);
        }

        [Fact]
        public void Constructor_AddressWithWhitespace_ShouldTrim()
        {
            var value = "  Rua das Flores, 123  ";
            var address = new Address(value);
            Assert.Equal("Rua das Flores, 123", address.Value);
        }
    }

    public class RecipientTests
    {
        [Fact]
        public void Constructor_ValidRecipient_ShouldCreateRecipient()
        {
            var name = "João Silva";
            var recipient = new Recipient(name);
            Assert.Equal(name, recipient.Name);
        }

        [Fact]
        public void Constructor_NullRecipient_ShouldCreateEmptyRecipient()
        {
            var recipient = new Recipient(null);
            Assert.Equal(string.Empty, recipient.Name);
        }

        [Fact]
        public void Constructor_EmptyRecipient_ShouldCreateEmptyRecipient()
        {
            var recipient = new Recipient("");
            Assert.Equal(string.Empty, recipient.Name);
        }

        [Fact]
        public void Constructor_WhitespaceRecipient_ShouldCreateEmptyRecipient()
        {
            var recipient = new Recipient("   ");
            Assert.Equal(string.Empty, recipient.Name);
        }

        [Fact]
        public void Constructor_RecipientWithWhitespace_ShouldTrim()
        {
            var name = "  Maria Santos  ";
            var recipient = new Recipient(name);
            Assert.Equal("Maria Santos", recipient.Name);
        }
    }
}
