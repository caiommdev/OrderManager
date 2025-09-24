using FluentAssertions;
using OrderManager.API.Application.Services.ShippingCalculators;
using OrderManager.API.Domain.ValueObjects;
using Xunit;

namespace OrderManager.Tests.Application.Services.ShippingCalculators
{
    public class EconomyShippingCalculatorTests
    {
        private readonly EconomyShippingCalculator _calculator;

        public EconomyShippingCalculatorTests()
        {
            _calculator = new EconomyShippingCalculator();
        }

        [Theory]
        [InlineData(3.0, 0.0)]
        [InlineData(5.0, 0.5)]
        [InlineData(10.0, 6.0)]
        [InlineData(2.5, 0.0)]
        public void CalculateShippingCost_WithWeightAboveFreeShippingLimit_ShouldReturnCorrectCost(double weight, decimal expectedCost)
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
        [InlineData(1.5)]
        [InlineData(1.9)]
        [InlineData(0.5)]
        public void CalculateShippingCost_WithWeightBelowFreeShippingLimit_ShouldReturnZero(double weight)
        {
            // Arrange
            var weightVO = new Weight(weight);

            // Act
            var result = _calculator.CalculateShippingCost(weightVO);

            // Assert
            result.Should().Be(0m);
        }

        [Theory]
        [InlineData(1.0)]
        [InlineData(1.5)]
        [InlineData(1.9)]
        [InlineData(0.5)]
        public void IsEligibleForFreeShipping_WithWeightBelowLimit_ShouldReturnTrue(double weight)
        {
            // Arrange
            var weightVO = new Weight(weight);

            // Act
            var result = _calculator.IsEligibleForFreeShipping(weightVO);

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(2.0)]
        [InlineData(2.1)]
        [InlineData(5.0)]
        [InlineData(10.0)]
        public void IsEligibleForFreeShipping_WithWeightAboveOrEqualLimit_ShouldReturnFalse(double weight)
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
            result.Should().Be("Econômico");
        }

        [Theory]
        [InlineData(4.54, 0.0)]
        [InlineData(4.55, 0.005)]
        public void CalculateShippingCost_WithEdgeCaseWeights_ShouldHandleMinimumCostCorrectly(double weight, decimal expectedCost)
        {
            // Arrange
            var weightVO = new Weight(weight);

            // Act
            var result = _calculator.CalculateShippingCost(weightVO);

            // Assert
            result.Should().BeApproximately(expectedCost, 0.001m);
        }
    }
}