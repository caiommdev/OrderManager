using FluentAssertions;
using OrderManager.API.Application.Services.ShippingCalculators;
using OrderManager.API.Domain.ValueObjects;
using Xunit;

namespace OrderManager.Tests.Application.Services.ShippingCalculators
{
    public class StandardShippingCalculatorTests
    {
        private readonly StandardShippingCalculator _calculator;

        public StandardShippingCalculatorTests()
        {
            _calculator = new StandardShippingCalculator();
        }

        [Theory]
        [InlineData(1.0, 1.2)]
        [InlineData(2.0, 2.4)]
        [InlineData(5.0, 6.0)]
        [InlineData(10.0, 12.0)]
        [InlineData(0.5, 0.6)]
        public void CalculateShippingCost_WithValidWeight_ShouldReturnCorrectCost(double weight, decimal expectedCost)
        {
            // Arrange
            var weightVO = new Weight(weight);

            // Act
            var result = _calculator.CalculateShippingCost(weightVO);

            // Assert
            result.Should().Be(expectedCost);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(2.0)]
        [InlineData(5.0)]
        [InlineData(10.0)]
        [InlineData(0.5)]
        public void IsEligibleForFreeShipping_WithAnyWeight_ShouldReturnFalse(double weight)
        {
            // Arrange
            var weightVO = new Weight(weight);

            // Act
            var result = _calculator.IsEligibleForFreeShipping(weightVO);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void GetShippingTypeName_ShouldReturnCorrectName()
        {
            // Act
            var result = _calculator.GetShippingTypeName();

            // Assert
            result.Should().Be("Padrão");
        }
    }
}