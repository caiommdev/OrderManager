using FluentAssertions;
using OrderManager.API.Application.Services.Validation;
using OrderManager.API.Domain.Exceptions;
using Xunit;

namespace OrderManager.Tests.Application.Services.Validation
{
    public class ValidationServiceTests
    {
        private readonly ValidationService _validationService;

        public ValidationServiceTests()
        {
            _validationService = new ValidationService();
        }

        #region ValidateWeight Tests

        [Theory]
        [InlineData(1.0)]
        [InlineData(0.1)]
        [InlineData(10.5)]
        [InlineData(100.0)]
        public void ValidateWeight_WithValidWeight_ShouldNotThrowException(double weight)
        {
            // Act & Assert
            var act = () => _validationService.ValidateWeight(weight);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(-1.0)]
        [InlineData(-10.5)]
        public void ValidateWeight_WithInvalidWeight_ShouldThrowInvalidWeightException(double weight)
        {
            // Act & Assert
            var act = () => _validationService.ValidateWeight(weight);
            act.Should().Throw<InvalidWeightException>()
               .WithMessage($"Peso inválido: {weight}. O peso deve ser maior que zero.");
        }

        [Fact]
        public void ValidateWeight_WithNaN_ShouldThrowInvalidWeightException()
        {
            // Arrange
            var weight = double.NaN;

            // Act & Assert
            var act = () => _validationService.ValidateWeight(weight);
            act.Should().Throw<InvalidWeightException>()
               .WithMessage($"Peso inválido: {weight}. O peso deve ser maior que zero.");
        }

        [Fact]
        public void ValidateWeight_WithPositiveInfinity_ShouldThrowInvalidWeightException()
        {
            // Arrange
            var weight = double.PositiveInfinity;

            // Act & Assert
            var act = () => _validationService.ValidateWeight(weight);
            act.Should().Throw<InvalidWeightException>()
               .WithMessage($"Peso inválido: {weight}. O peso deve ser maior que zero.");
        }

        [Fact]
        public void ValidateWeight_WithNegativeInfinity_ShouldThrowInvalidWeightException()
        {
            // Arrange
            var weight = double.NegativeInfinity;

            // Act & Assert
            var act = () => _validationService.ValidateWeight(weight);
            act.Should().Throw<InvalidWeightException>()
               .WithMessage($"Peso inválido: {weight}. O peso deve ser maior que zero.");
        }

        #endregion

        #region ValidateAddress Tests

        [Theory]
        [InlineData("Rua das Flores, 123")]
        [InlineData("Av. Paulista, 1000 - São Paulo")]
        [InlineData("123 Main St")]
        [InlineData("A")]
        public void ValidateAddress_WithValidAddress_ShouldNotThrowException(string address)
        {
            // Act & Assert
            var act = () => _validationService.ValidateAddress(address);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void ValidateAddress_WithEmptyOrWhitespaceAddress_ShouldThrowInvalidAddressException(string address)
        {
            // Act & Assert
            var act = () => _validationService.ValidateAddress(address);
            act.Should().Throw<InvalidAddressException>()
               .WithMessage($"Endereço inválido: '{address}'. O endereço não pode ser nulo ou vazio.");
        }

        [Fact]
        public void ValidateAddress_WithNullAddress_ShouldThrowInvalidAddressException()
        {
            // Arrange
            string? address = null;

            // Act & Assert
            var act = () => _validationService.ValidateAddress(address!);
            act.Should().Throw<InvalidAddressException>()
               .WithMessage("Endereço inválido: 'null'. O endereço não pode ser nulo ou vazio.");
        }

        #endregion

        #region ValidateRecipient Tests

        [Theory]
        [InlineData("João Silva")]
        [InlineData("Maria")]
        [InlineData("José da Silva Pereira")]
        [InlineData("A")]
        public void ValidateRecipient_WithValidRecipient_ShouldNotThrowException(string recipient)
        {
            // Act & Assert
            var act = () => _validationService.ValidateRecipient(recipient);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void ValidateRecipient_WithEmptyOrWhitespaceRecipient_ShouldThrowInvalidRecipientException(string recipient)
        {
            // Act & Assert
            var act = () => _validationService.ValidateRecipient(recipient);
            act.Should().Throw<InvalidRecipientException>()
               .WithMessage($"Destinatário inválido: '{recipient}'. O destinatário não pode ser nulo ou vazio.");
        }

        [Fact]
        public void ValidateRecipient_WithNullRecipient_ShouldThrowInvalidRecipientException()
        {
            // Arrange
            string? recipient = null;

            // Act & Assert
            var act = () => _validationService.ValidateRecipient(recipient!);
            act.Should().Throw<InvalidRecipientException>()
               .WithMessage("Destinatário inválido: 'null'. O destinatário não pode ser nulo ou vazio.");
        }

        #endregion

        #region ValidateShippingType Tests

        [Theory]
        [InlineData("PAD")]
        [InlineData("EXP")]
        [InlineData("ECO")]
        public void ValidateShippingType_WithValidShippingType_ShouldNotThrowException(string shippingType)
        {
            // Act & Assert
            var act = () => _validationService.ValidateShippingType(shippingType);
            act.Should().NotThrow();
        }

        [Theory]
        [InlineData("INVALID")]
        [InlineData("XYZ")]
        [InlineData("123")]
        public void ValidateShippingType_WithInvalidShippingType_ShouldThrowUnsupportedShippingTypeException(string shippingType)
        {
            // Act & Assert
            var act = () => _validationService.ValidateShippingType(shippingType);
            act.Should().Throw<UnsupportedShippingTypeException>()
               .WithMessage($"Tipo de frete não suportado: '{shippingType}'.");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void ValidateShippingType_WithEmptyOrWhitespaceShippingType_ShouldThrowUnsupportedShippingTypeException(string shippingType)
        {
            // Act & Assert
            var act = () => _validationService.ValidateShippingType(shippingType);
            act.Should().Throw<UnsupportedShippingTypeException>()
               .WithMessage($"Tipo de frete não suportado: '{shippingType}'.");
        }

        [Fact]
        public void ValidateShippingType_WithNullShippingType_ShouldThrowUnsupportedShippingTypeException()
        {
            // Arrange
            string? shippingType = null;

            // Act & Assert
            var act = () => _validationService.ValidateShippingType(shippingType!);
            act.Should().Throw<UnsupportedShippingTypeException>()
               .WithMessage("Tipo de frete não suportado: 'null'.");
        }

        #endregion
    }
}