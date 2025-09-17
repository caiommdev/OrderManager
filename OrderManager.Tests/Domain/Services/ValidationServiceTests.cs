using WebApplication1.Domain.Exceptions;
using WebApplication1.Domain.Services.Validation;
using Xunit;

namespace OrderManager.Tests.Domain.Services
{
    public class ValidationServiceTests
    {
        private readonly ValidationService _validationService = new();

        [Fact]
        public void ValidateWeight_ValidWeight_ShouldNotThrow()
        {
            var weight = 5.0;
            var exception = Record.Exception(() => _validationService.ValidateWeight(weight));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10.5)]
        public void ValidateWeight_InvalidWeight_ShouldThrowException(double invalidWeight)
        {
            Assert.Throws<InvalidWeightException>(() => _validationService.ValidateWeight(invalidWeight));
        }

        [Fact]
        public void ValidateAddress_ValidAddress_ShouldNotThrow()
        {
            var address = "Rua das Flores, 123";
            var exception = Record.Exception(() => _validationService.ValidateAddress(address));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateAddress_InvalidAddress_ShouldThrowException(string invalidAddress)
        {
            Assert.Throws<InvalidAddressException>(() => _validationService.ValidateAddress(invalidAddress));
        }

        [Fact]
        public void ValidateRecipient_ValidRecipient_ShouldNotThrow()
        {
            var recipient = "João Silva";
            var exception = Record.Exception(() => _validationService.ValidateRecipient(recipient));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateRecipient_InvalidRecipient_ShouldThrowException(string invalidRecipient)
        {
            Assert.Throws<InvalidRecipientException>(() => _validationService.ValidateRecipient(invalidRecipient));
        }
    }
}
